#if INTERACTIVE
#load "../src/ProvidedTypes.fsi" "../src/ProvidedTypes.fs"
#load "../src/ProvidedTypesTesting.fs"

#else
module FSharp.TypeProviders.SDK.Tests.BasicGenerativeTests
#endif

open System.Reflection
open System.IO
open ProviderImplementation
open ProviderImplementation.ProvidedTypes
open ProviderImplementation.ProvidedTypesTesting
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Core.CompilerServices
open Xunit

#nowarn "760" // IDisposable needs new

#if !NO_GENERATIVE

[<TypeProvider>]
type GenerativePropertyProviderWithStaticParams (config : TypeProviderConfig) as this =
    inherit TypeProviderForNamespaces ()

    let ns = "StaticProperty.Provided"
    let asm = Assembly.GetExecutingAssembly()

    let ctxt = ProvidedTypesContext.Create(config)
    let createType (typeName, n:int) =
        let myAssem = ProvidedAssembly(ctxt)
        let myType = ctxt.ProvidedTypeDefinition(asm, ns, typeName, Some typeof<obj>, isErased=false)
        let embedString = "test"
        // Special TPSDK support for embedding Decimal values
        let embedM = 5M
        // Special TPSDK support for embedding System.DateTime values
        let embedDT = System.DateTime.Now
        // Special TPSDK support for embedding System.DateTimeOffset values
        let embedDTO = System.DateTimeOffset.Now
        // Special TPSDK support for embedding System.Type values
        let embedType = typeof<int>
        let testCode args = 
             <@@ // NewArray
                 let arr = [| 1;2;3;4 |]
                 // Coerce
                 let arr2 = (box arr :?> int[])
                 let s = "hello world"
                 // Literal field
                 let s2 = System.DayOfWeek.Friday
                 // NewObj on default ctor - this is not yet supported in generative
                 // let s3 = System.DateTime()
                 // NewObj on value type
                 let s4 = System.DateTime(100L)
                 // NewObj on reference type
                 let s5 = System.Object()
                 // NewObj on generic reference type
                 let s6 = System.Collections.Generic.List<int>()
                 // NewObj on generic reference type
                 let s7 = System.Collections.Generic.Dictionary<int,int>()

                 //Arithmetic - note, operations such as + are emitted as a call to the method in the F# library, even over integers
                 let z1 = 1 + 1 - 1 * 1 / 1
                 let z2 = 1u + 1u - 1u * 1u / 1u
                 let z3 = 1L + 1L - 1L * 1L / 1L
                 //Arithmetic (decimals)
                 let z4 = 1M + 1M - 1M * 1M / 1M
                 //Lambda
                 let f1  = (fun (x:int) -> x + 1)
                 let f2  = (fun (x:int) (y:int) -> x + y + 1)
                 let f3  = (fun (x:int) (y:int) (z:int) -> x + y + z + 1)
                 // Application
                 let z5 = f1 3 + f2 3 4 + f3 4 5 6
                 // Const, Tuple
                 let q1 = (embedM, embedString)
                 let q2 = (embedM, embedString, embedDT, embedType, embedDTO, (1,2,3))
                 // WhileLoop
                 while false  do ()
                 // FastIntegerForLoop
                 for i in 0 .. 100 do 
                    ignore ()
                 // // ForLoop - the TryFinally is not yet supported
                 // for i in 0L .. 100L do 
                 //    ignore ()
                 //IfThenElse one branch
                 if s = "" then 
                     failwith "test"
                 //IfThenElse
                 if s = "" then 
                   [] 
                 else
                   Set.toList (Set.ofList [ "Hello world" ]) @@>    
        let adderCode (args: Expr list) = <@@ ignore (%%(args.[1]): System.EventHandler) @@>
        let removerCode (args: Expr list) = <@@ ignore (%%(args.[1]) : System.EventHandler) @@>
        let setterCode (args: Expr list) = <@@ ignore (%%(args.[1]) : string list) @@>

        let myProp = ctxt.ProvidedProperty("MyStaticProperty", typeof<string list>, isStatic = true, getterCode = testCode)
        let myProp2 = ctxt.ProvidedProperty("MyInstaceProperty", typeof<string list>, isStatic = false, getterCode = testCode, setterCode = setterCode)
        let myMeth1 = ctxt.ProvidedMethod("MyStaticMethod", [], typeof<string list>, isStatic = true, invokeCode = testCode)
        let myMeth2 = ctxt.ProvidedMethod("MyInstanceMethod", [], typeof<string list>, isStatic = false, invokeCode = testCode)
        let myEvent1 = ctxt.ProvidedEvent("MyEvent", typeof<System.EventHandler>, isStatic = false, adderCode = adderCode, removerCode = removerCode)
        myType.AddMembers [ (myProp :> MemberInfo); (myProp2 :> MemberInfo); (myMeth1 :> MemberInfo); (myMeth2 :> MemberInfo); (myEvent1 :> MemberInfo)]
        myAssem.AddTypes [myType]
        myType

    do
        let myType = ctxt.ProvidedTypeDefinition(asm, ns, "MyType", Some typeof<obj>)
        let parameters = [ ctxt.ProvidedStaticParameter("Count", typeof<int>) 
                           ctxt.ProvidedStaticParameter("Count2", typeof<int>, 3) ]
        myType.DefineStaticParameters(parameters, (fun typeName args -> createType(typeName, (args.[0] :?> int) + (args.[1] :?> int))))

        this.AddNamespace(ns, [myType])



[<Fact>]
let ``GenerativePropertyProviderWithStaticParams generates for correctly``() : unit  = 
    let testCases = 
        [("3.259.3.1", (fun _ ->  Targets.hasPortable259Assemblies()), Targets.Portable259FSharp31Refs)
         ("3.259.4.0", (fun _ ->  Targets.hasPortable259Assemblies() && Targets.supportsFSharp40), Targets.Portable259FSharp40Refs)
         ("4.3.1.0", (fun _ ->  Targets.supportsFSharp40), Targets.DotNet45FSharp31Refs)
         ("4.4.0.0", (fun _ ->  Targets.supportsFSharp40), Targets.DotNet45FSharp40Refs) ]
    for (desc, supports, refs) in testCases do
        if supports() then 
            let staticArgs = [|  box 3; box 4  |] 
            let runtimeAssemblyRefs = refs()
            let runtimeAssembly = runtimeAssemblyRefs.[0]
            let cfg = Testing.MakeSimulatedTypeProviderConfig (__SOURCE_DIRECTORY__, runtimeAssembly, runtimeAssemblyRefs) 
            let typeProviderForNamespaces = GenerativePropertyProviderWithStaticParams cfg :> TypeProviderForNamespaces
            let providedTypeDefinition = typeProviderForNamespaces.Namespaces |> Seq.last |> snd |> Seq.last
            let typeName = providedTypeDefinition.Name + (staticArgs |> Seq.map (fun s -> ",\"" + (if isNull s then "" else s.ToString()) + "\"") |> Seq.reduce (+))

            let t = providedTypeDefinition.ApplyStaticArguments(typeName, staticArgs)
            Assert.True(match t.Assembly with :? ProvidedAssembly -> true | _ -> false)
            let assemContents = (typeProviderForNamespaces :> ITypeProvider).GetGeneratedAssemblyContents(t.Assembly)
            Assert.NotEqual(assemContents.Length, 0)
            let res = [| for r in t.Assembly.GetReferencedAssemblies() -> r.ToString() |] |> String.concat ","
            printfn "----- %s ------- " desc 
            printfn "compilation references for FSharp.Core target %s = %A" desc runtimeAssemblyRefs
            printfn "assembly references for FSharp.Core target %s = %s" desc res
            for (desc2, _, _) in testCases do 
                let contains = res.Contains("FSharp.Core, Version="+desc2)
                if contains = (desc = desc2) then ()
                elif contains then failwith ("unexpected reference to FSharp.Core, Version="+desc+"in output")
                else failwith ("failed to find reference to FSharp.Core, Version="+desc2+"in output" )


    // TEST: Register binary
    // TEST: Many more F# constructs in generated code, giveing full coverage
#endif
