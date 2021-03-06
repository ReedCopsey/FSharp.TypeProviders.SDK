﻿// Copyright (c) Microsoft Corporation 2005-2014 and other contributors.
// This sample code is provided "as is" without warranty of any kind.
// We disclaim all warranties, either express or implied, including the
// warranties of merchantability and fitness for a particular purpose.
//
// This file contains a set of helper types and methods for providing types in an implementation
// of ITypeProvider.
//
// This code has been modified and is appropriate for use in conjunction with the F# 3.0-4.0 releases


namespace ProviderImplementation.ProvidedTypes

    open System
    open System.Collections.Generic
    open System.Reflection
    open System.Linq.Expressions
    open Microsoft.FSharp.Quotations
    open Microsoft.FSharp.Core.CompilerServices

    /// Represents an erased provided parameter
    [<Class>]
    type ProvidedParameter =
        inherit ParameterInfo

        /// Indicates if the parameter is marked as ParamArray
        member IsParamArray: bool with get,set

        /// Indicates if the parameter is marked as ReflectedDefinition
        member IsReflectedDefinition: bool with get,set

        /// Indicates if the parameter has a default value
        member HasDefaultParameterValue: bool

    /// Represents a provided static parameter.
    [<Class>]
    type ProvidedStaticParameter =
        inherit ParameterInfo

        /// Add XML documentation information to this provided constructor
        member AddXmlDoc: xmlDoc: string -> unit

        /// Add XML documentation information to this provided constructor, where the computation of the documentation is delayed until necessary
        member AddXmlDocDelayed: xmlDocFunction: (unit -> string) -> unit


    /// Represents an erased provided constructor.
    [<Class>]
    type ProvidedConstructor =
        inherit ConstructorInfo

        /// Add a 'Obsolete' attribute to this provided constructor
        member AddObsoleteAttribute: message: string * ?isError: bool -> unit

        /// Add XML documentation information to this provided constructor
        member AddXmlDoc: xmlDoc: string -> unit

        /// Add XML documentation information to this provided constructor, where the computation of the documentation is delayed until necessary
        member AddXmlDocDelayed: xmlDocFunction: (unit -> string) -> unit

        /// Add XML documentation information to this provided constructor, where the documentation is re-computed  every time it is required.
        member AddXmlDocComputed: xmlDocFunction: (unit -> string) -> unit

        /// Set the target and arguments of the base constructor call. Only used for generated types.
        member BaseConstructorCall: (Expr list -> ConstructorInfo * Expr list) with set

        /// Set a flag indicating that the constructor acts like an F# implicit constructor, so the
        /// parameters of the constructor become fields and can be accessed using Expr.GlobalVar with the
        /// same name.
        member IsImplicitCtor: bool with get,set

        /// Add definition location information to the provided constructor.
        member AddDefinitionLocation: line:int * column:int * filePath:string -> unit

        member IsTypeInitializer: bool with get,set

        /// This method is for internal use only in the type provider SDK
        member internal GetInvokeCodeInternal: isGenerated: bool * convToTgt: (Type -> Type) -> (Expr [] -> Expr)



    [<Class>]
    type ProvidedMethod =
        inherit MethodInfo

        /// Add XML documentation information to this provided method
        member AddObsoleteAttribute: message: string * ?isError: bool -> unit

        /// Add XML documentation information to this provided constructor
        member AddXmlDoc: xmlDoc: string -> unit

        /// Add XML documentation information to this provided constructor, where the computation of the documentation is delayed until necessary
        member AddXmlDocDelayed: xmlDocFunction: (unit -> string) -> unit

        /// Add XML documentation information to this provided constructor, where the computation of the documentation is delayed until necessary
        /// The documentation is re-computed  every time it is required.
        member AddXmlDocComputed: xmlDocFunction: (unit -> string) -> unit

        member AddMethodAttrs: attributes:MethodAttributes -> unit

        /// Set the method attributes of the method. By default these are simple 'MethodAttributes.Public'
        member SetMethodAttrs: attributes:MethodAttributes -> unit

        /// Add definition location information to the provided type definition.
        member AddDefinitionLocation: line:int * column:int * filePath:string -> unit

        /// Add a custom attribute to the provided method definition.
        member AddCustomAttribute: CustomAttributeData -> unit

        /// Define the static parameters available on a statically parameterized method
        member DefineStaticParameters: parameters: ProvidedStaticParameter list * instantiationFunction: (string -> obj[] -> ProvidedMethod) -> unit

        /// This method is for internal use only in the type provider SDK
        member internal GetInvokeCodeInternal: isGenerated: bool * convToTgt: (Type -> Type) -> (Expr [] -> Expr)


    /// Represents an erased provided property.
    [<Class>]
    type ProvidedProperty =
        inherit PropertyInfo

        /// Add a 'Obsolete' attribute to this provided property
        member AddObsoleteAttribute: message: string * ?isError: bool -> unit

        /// Add XML documentation information to this provided constructor
        member AddXmlDoc: xmlDoc: string -> unit

        /// Add XML documentation information to this provided constructor, where the computation of the documentation is delayed until necessary
        member AddXmlDocDelayed: xmlDocFunction: (unit -> string) -> unit

        /// Add XML documentation information to this provided constructor, where the computation of the documentation is delayed until necessary
        /// The documentation is re-computed  every time it is required.
        member AddXmlDocComputed: xmlDocFunction: (unit -> string) -> unit

        /// Get or set a flag indicating if the property is static.
        member IsStatic: bool

        /// Add definition location information to the provided type definition.
        member AddDefinitionLocation: line:int * column:int * filePath:string -> unit

        /// Add a custom attribute to the provided property definition.
        member AddCustomAttribute: CustomAttributeData -> unit


    /// Represents an erased provided property.
    [<Class>]
    type ProvidedEvent =
        inherit EventInfo

        /// Add XML documentation information to this provided constructor
        member AddXmlDoc: xmlDoc: string -> unit

        /// Add XML documentation information to this provided constructor, where the computation of the documentation is delayed until necessary
        member AddXmlDocDelayed: xmlDocFunction: (unit -> string) -> unit

        /// Add XML documentation information to this provided constructor, where the computation of the documentation is delayed until necessary
        /// The documentation is re-computed  every time it is required.
        member AddXmlDocComputed: xmlDocFunction: (unit -> string) -> unit

        /// Get a flag indicating if the property is static.
        member IsStatic: bool with get

        /// Add definition location information to the provided type definition.
        member AddDefinitionLocation: line:int * column:int * filePath:string -> unit


    /// Represents an erased provided field.
    [<Class>]
    type ProvidedLiteralField =
        inherit FieldInfo

        /// Add a 'Obsolete' attribute to this provided field
        member AddObsoleteAttribute: message: string * ?isError: bool -> unit

        /// Add XML documentation information to this provided field
        member AddXmlDoc: xmlDoc: string -> unit

        /// Add XML documentation information to this provided field, where the computation of the documentation is delayed until necessary
        member AddXmlDocDelayed: xmlDocFunction: (unit -> string) -> unit

        /// Add XML documentation information to this provided field, where the computation of the documentation is delayed until necessary
        /// The documentation is re-computed  every time it is required.
        member AddXmlDocComputed: xmlDocFunction: (unit -> string) -> unit

        /// Add definition location information to the provided field.
        member AddDefinitionLocation: line:int * column:int * filePath:string -> unit


    /// Represents an erased provided field.
    [<Class>]
    type ProvidedField =
        inherit FieldInfo

        /// Add a 'Obsolete' attribute to this provided field
        member AddObsoleteAttribute: message: string * ?isError: bool -> unit

        /// Add XML documentation information to this provided field
        member AddXmlDoc: xmlDoc: string -> unit

        /// Add XML documentation information to this provided field, where the computation of the documentation is delayed until necessary
        member AddXmlDocDelayed: xmlDocFunction: (unit -> string) -> unit

        /// Add XML documentation information to this provided field, where the computation of the documentation is delayed until necessary
        /// The documentation is re-computed  every time it is required.
        member AddXmlDocComputed: xmlDocFunction: (unit -> string) -> unit

        /// Add definition location information to the provided field definition.
        member AddDefinitionLocation: line:int * column:int * filePath:string -> unit

        member SetFieldAttributes: attributes: FieldAttributes -> unit


    /// Represents the type constructor in a provided symbol type.
    [<NoComparison>]
    type ProvidedSymbolKind =

        /// Indicates that the type constructor is for a single-dimensional array
        | SDArray

        /// Indicates that the type constructor is for a multi-dimensional array
        | Array of int

        /// Indicates that the type constructor is for pointer types
        | Pointer

        /// Indicates that the type constructor is for byref types
        | ByRef

        /// Indicates that the type constructor is for named generic types
        | Generic of Type

        /// Indicates that the type constructor is for abbreviated types
        | FSharpTypeAbbreviation of (Assembly * string * string[])

    /// Represents an array or other symbolic type involving a provided type as the argument.
    /// See the type provider spec for the methods that must be implemented.
    /// Note that the type provider specification does not require us to implement pointer-equality for provided types.
    [<Class>]
    type ProvidedSymbolType =
        inherit TypeDelegator

        //interface IReflectableType

        /// Returns the kind of this symbolic type
        member Kind: ProvidedSymbolKind

        /// Return the provided types used as arguments of this symbolic type
        member Args: list<Type>

        /// For example, kg
        member IsFSharpTypeAbbreviation: bool

        /// For example, int<kg> or int<kilogram>
        member IsFSharpUnitAnnotated: bool

    /// Helpers to build symbolic provided types
    [<Class>]
    type ProvidedTypeBuilder =

        /// Like typ.MakeGenericType, but will also work with unit-annotated types
        static member MakeGenericType: genericTypeDefinition: Type * genericArguments: Type list -> Type

        /// Like methodInfo.MakeGenericMethod, but will also work with unit-annotated types and provided types
        static member MakeGenericMethod: genericMethodDefinition: MethodInfo * genericArguments: Type list -> MethodInfo


    /// Helps create erased provided unit-of-measure annotations.
    [<Class>]
    type ProvidedMeasureBuilder =

        /// The ProvidedMeasureBuilder for building measures.
        static member Default: ProvidedMeasureBuilder

        /// Gets the measure indicating the "1" unit of measure, that is the unitless measure.
        member One: Type

        /// Returns the measure indicating the product of two units of measure, e.g. kg * m
        member Product: measure1: Type * measure2: Type  -> Type

        /// Returns the measure indicating the inverse of two units of measure, e.g. 1 / s
        member Inverse: denominator: Type -> Type

        /// Returns the measure indicating the ratio of two units of measure, e.g. kg / m
        member Ratio: numerator: Type * denominator: Type -> Type

        /// Returns the measure indicating the square of a unit of measure, e.g. m * m
        member Square: ``measure``: Type -> Type

        /// Returns the measure for an SI unit from the F# core library, where the string is in capitals and US spelling, e.g. Meter
        member SI: unitName:string -> Type

        /// Returns a type where the type has been annotated with the given types and/or units-of-measure.
        /// e.g. float<kg>, Vector<int, kg>
        member AnnotateType: basic: Type * argument: Type list -> Type


    /// Represents a provided type definition.
    [<Class>]
    type ProvidedTypeDefinition =
        inherit TypeDelegator

        /// Add the given type as an implemented interface.
        member AddInterfaceImplementation: interfaceType: Type -> unit

        /// Add the given function as a set of on-demand computed interfaces.
        member AddInterfaceImplementationsDelayed: interfacesFunction:(unit -> Type list)-> unit

        /// Specifies that the given method body implements the given method declaration.
        member DefineMethodOverride: methodInfoBody: ProvidedMethod * methodInfoDeclaration: MethodInfo -> unit

        /// Add a 'Obsolete' attribute to this provided type definition
        member AddObsoleteAttribute: message: string * ?isError: bool -> unit

        /// Add XML documentation information to this provided constructor
        member AddXmlDoc: xmlDoc: string -> unit

        /// Set the base type
        member SetBaseType: Type -> unit

        /// Set the base type to a lazily evaluated value. Use this to delay realization of the base type as late as possible.
        member SetBaseTypeDelayed: baseTypeFunction:(unit -> Type) -> unit

        /// Set underlying type for generated enums
        member SetEnumUnderlyingType: Type -> unit

        /// Add XML documentation information to this provided constructor, where the computation of the documentation is delayed until necessary.
        /// The documentation is only computed once.
        member AddXmlDocDelayed: xmlDocFunction: (unit -> string) -> unit

        /// Add XML documentation information to this provided constructor, where the computation of the documentation is delayed until necessary
        /// The documentation is re-computed  every time it is required.
        member AddXmlDocComputed: xmlDocFunction: (unit -> string) -> unit

        /// Set the attributes on the provided type. This fully replaces the default TypeAttributes.
        member SetAttributes: TypeAttributes -> unit

        /// Add a method, property, nested type or other member to a ProvidedTypeDefinition
        member AddMember: memberInfo:MemberInfo      -> unit

        /// Add a set of members to a ProvidedTypeDefinition
        member AddMembers: memberInfos:list<#MemberInfo> -> unit

        /// Add a member to a ProvidedTypeDefinition, delaying computation of the members until required by the compilation context.
        member AddMemberDelayed: memberFunction:(unit -> #MemberInfo)      -> unit

        /// Add a set of members to a ProvidedTypeDefinition, delaying computation of the members until required by the compilation context.
        member AddMembersDelayed: membersFunction:(unit -> list<#MemberInfo>) -> unit

        /// Add the types of the generated assembly as generative types, where types in namespaces get hierarchically positioned as nested types.
        member AddAssemblyTypesAsNestedTypesDelayed: assemblyFunction:(unit -> Assembly) -> unit

        /// Define the static parameters available on a statically parameterized type
        member DefineStaticParameters: parameters: ProvidedStaticParameter list * instantiationFunction: (string -> obj[] -> ProvidedTypeDefinition) -> unit

        /// Add definition location information to the provided type definition.
        member AddDefinitionLocation: line:int * column:int * filePath:string -> unit

        /// Suppress Object entries in intellisense menus in instances of this provided type
        member HideObjectMethods: bool 

        /// Disallows the use of the null literal.
        member NonNullable: bool

        /// Get a flag indicating if the ProvidedTypeDefinition is erased
        member IsErased: bool 

        /// Get or set a flag indicating if the ProvidedTypeDefinition has type-relocation suppressed
        [<Experimental("SuppressRelocation is a workaround and likely to be removed")>]
        member SuppressRelocation: bool  with get,set

        // This method is used by Debug.fs
        member ApplyStaticArguments: name:string * args:obj[] -> ProvidedTypeDefinition

        /// Add a custom attribute to the provided type definition.
        member AddCustomAttribute: CustomAttributeData -> unit

        /// Emulate the F# type provider type erasure mechanism to get the
        /// actual (erased) type. We erase ProvidedTypes to their base type
        /// and we erase array of provided type to array of base type. In the
        /// case of generics all the generic type arguments are also recursively
        /// replaced with the erased-to types
        static member EraseType: typ:Type -> Type

        /// Get or set a utility function to log the creation of root Provided Type. Used to debug caching/invalidation.
        static member Logger: (string -> unit) option ref


    [<Class>]
    type ProvidedTypesContext = 
        
        /// Create a context for providing types for a particular rntime target.
        /// Specific assembly renaming replacements can be provided using assemblyReplacementMap.
        static member Create : cfg: TypeProviderConfig * ?assemblyReplacementMap : seq<string*string> -> ProvidedTypesContext

        /// Create a new provided static parameter, for use with DefineStaticParamaeters on a provided type definition.
        ///
        /// When making a cross-targeting type provider, use this method instead of the ProvidedParameter constructor from ProvidedTypes
        member ProvidedStaticParameter: parameterName: string * parameterType: Type * ?parameterDefaultValue: obj -> ProvidedStaticParameter

        /// Create a new provided field. It is not initially associated with any specific provided type definition.
        ///
        /// When making a cross-targeting type provider, use this method instead of the ProvidedProperty constructor from ProvidedTypes
        member ProvidedField: fieldName: string * fieldType: Type -> ProvidedField

        /// Create a new provided literal field. It is not initially associated with any specific provided type definition.
        ///
        /// When making a cross-targeting type provider, use this method instead of the ProvidedProperty constructor from ProvidedTypes
        member ProvidedLiteralField: fieldName: string * fieldType: Type * literalValue:obj -> ProvidedLiteralField

        /// Create a new provided parameter.
        ///
        /// When making a cross-targeting type provider, use this method instead of the ProvidedProperty constructor from ProvidedTypes
        member ProvidedParameter: parameterName: string * parameterType: Type * ?isOut: bool * ?optionalValue: obj -> ProvidedParameter

        /// Create a new provided property. It is not initially associated with any specific provided type definition.
        ///
        /// When making a cross-targeting type provider, use this method instead of the ProvidedProperty constructor from ProvidedTypes
        member ProvidedProperty: propertyName: string * propertyType: Type * ?isStatic: bool * ?getterCode: (Expr list -> Expr) * ?setterCode: (Expr list -> Expr) * ?parameters: ProvidedParameter list -> ProvidedProperty

        /// Create a new provided event. It is not initially associated with any specific provided type definition.
        ///
        /// When making a cross-targeting type provider, use this method instead of the ProvidedProperty constructor from ProvidedTypes
        member ProvidedEvent: eventName: string * eventHandlerType: Type * ?isStatic: bool * ?adderCode: (Expr list -> Expr) * ?removerCode: (Expr list -> Expr) -> ProvidedEvent

        /// When making a cross-targeting type provider, use this method instead of the ProvidedConstructor constructor from ProvidedTypes
        member ProvidedConstructor: parameters: ProvidedParameter list * ?invokeCode: (Expr list -> Expr) -> ProvidedConstructor

        /// When making a cross-targeting type provider, use this method instead of the ProvidedMethod constructor from ProvidedTypes
        member ProvidedMethod: methodName: string * parameters: ProvidedParameter list * returnType: Type * ?isStatic: bool * ?invokeCode: (Expr list -> Expr)  -> ProvidedMethod

        /// When making a cross-targeting type provider, use this method instead of the corresponding ProvidedTypeDefinition constructor from ProvidedTypes
        member ProvidedTypeDefinition: className: string * baseType: Type option * ?hideObjectMethods: bool * ?nonNullable: bool * ?isErased: bool -> ProvidedTypeDefinition

        /// When making a cross-targeting type provider, use this method instead of the corresponding ProvidedTypeDefinition constructor from ProvidedTypes
        member ProvidedTypeDefinition: assembly: Assembly * namespaceName: string * className: string * baseType: Type option * ?hideObjectMethods: bool * ?nonNullable: bool * ?isErased: bool  -> ProvidedTypeDefinition

        /// When making a cross-targeting type provider, use this method instead of ProvidedTypeBuilder.MakeGenericType
        member MakeGenericType: genericTypeDefinition: Type * genericArguments: Type list -> Type

        /// When making a cross-targeting type provider, use this method instead of ProvidedTypeBuilder.MakeGenericMethod
        member MakeGenericMethod: genericMethodDefinition: MethodInfo * genericArguments: Type list -> MethodInfo

        /// Try to find the given assembly in the context
        member TryBindAssembly: aref: AssemblyName -> Choice<Assembly, exn> 

        /// Try to find the given assembly in the context
        member TryBindAssemblyBySimpleName: assemblyName: string  -> Choice<Assembly, exn> 

        /// Get the list of referenced assemblies determined by the type provider configuration
        member ReferencedAssemblyPaths: string list

        /// Get the resolved referenced assemblies determined by the type provider configuration
        member ReferencedAssemblies : Assembly[]

        /// Try to get the version of FSharp.Core referenced. May raise an exception if FSharp.Core has not been correctly resolved
        member FSharpCoreAssemblyVersion: Version



    /// A base type providing default implementations of type provider functionality when all provided
    /// types are of type ProvidedTypeDefinition.
    type TypeProviderForNamespaces =

        /// Initializes a type provider to provide the types in the given namespace.
        new: namespaceName:string * types: ProvidedTypeDefinition list -> TypeProviderForNamespaces

        /// Initializes a type provider
        new: unit -> TypeProviderForNamespaces

        /// Invoked by the type provider to add a namespace of provided types in the specification of the type provider.
        member AddNamespace: namespaceName:string * types: ProvidedTypeDefinition list -> unit

        /// Invoked by the type provider to get all provided namespaces with their provided types.
        member Namespaces: seq<string * ProvidedTypeDefinition list>

        /// Invoked by the type provider to invalidate the information provided by the provider
        member Invalidate: unit -> unit

        /// Invoked by the host of the type provider to get the static parameters for a method.
        member GetStaticParametersForMethod: MethodBase -> ParameterInfo[]

        /// Invoked by the host of the type provider to apply the static argumetns for a method.
        member ApplyStaticArgumentsForMethod: MethodBase * string * obj[] -> MethodBase

#if !FX_NO_LOCAL_FILESYSTEM
        /// AssemblyResolve handler. Default implementation searches <assemblyname>.dll file in registered folders
        abstract ResolveAssembly: ResolveEventArgs -> Assembly
        default ResolveAssembly: ResolveEventArgs -> Assembly

        /// Registers custom probing path that can be used for probing assemblies
        member RegisterProbingFolder: folder: string -> unit

        /// Registers location of RuntimeAssembly (from TypeProviderConfig) as probing folder
        member RegisterRuntimeAssemblyLocationAsProbingFolder: config: TypeProviderConfig -> unit

#endif

        [<CLIEvent>]
        member Disposing: IEvent<EventHandler,EventArgs>

        interface ITypeProvider


#if !NO_GENERATIVE
    /// An internal type used in the implementation of ProvidedAssembly
    [<Class>]
    type ContextAssembly =

        inherit Assembly

    /// A provided generated assembly
    type ProvidedAssembly =

        inherit ContextAssembly

        /// Create a provided generated assembly
        new: assemblyName: AssemblyName * assemblyFileName:string * context:ProvidedTypesContext -> ProvidedAssembly

        /// Create a provided generated assembly using a temporary file as the interim assembly storage
        new: context:ProvidedTypesContext -> ProvidedAssembly

        /// Emit the given provided type definitions as part of the assembly
        /// and adjust the 'Assembly' property of all provided type definitions to return that
        /// assembly.
        ///
        /// The assembly is only emitted when the Assembly property on the root type is accessed for the first time.
        /// The host F# compiler does this when processing a generative type declaration for the type.
        member AddTypes: types: ProvidedTypeDefinition list -> unit

        /// <summary>
        /// Emit the given nested provided type definitions as part of the assembly.
        /// and adjust the 'Assembly' property of all provided type definitions to return that
        /// assembly.
        /// </summary>
        /// <param name="enclosingTypeNames">A path of type names to wrap the generated types. The generated types are then generated as nested types.</param>
        member AddNestedTypes: types: ProvidedTypeDefinition list * enclosingGeneratedTypeNames: string list -> unit

#if !FX_NO_LOCAL_FILESYSTEM
        /// Register that a given file is a provided generated assembly
        static member RegisterGenerated: context: ProvidedTypesContext * fileName: string -> Assembly
#endif

#endif



    module internal UncheckedQuotations =

      type Expr with
        static member NewDelegateUnchecked: ty:Type * vs:Var list * body:Expr -> Expr
        static member NewObjectUnchecked: cinfo:ConstructorInfo * args:Expr list -> Expr
        static member NewArrayUnchecked: elementType:Type * elements:Expr list -> Expr
        static member CallUnchecked: minfo:MethodInfo * args:Expr list -> Expr
        static member CallUnchecked: obj:Expr * minfo:MethodInfo * args:Expr list -> Expr
        static member ApplicationUnchecked: f:Expr * x:Expr -> Expr
        static member PropertyGetUnchecked: pinfo:PropertyInfo * args:Expr list -> Expr
        static member PropertyGetUnchecked: obj:Expr * pinfo:PropertyInfo * ?args:Expr list -> Expr
        static member PropertySetUnchecked: pinfo:PropertyInfo * value:Expr * ?args:Expr list -> Expr
        static member PropertySetUnchecked: obj:Expr * pinfo:PropertyInfo * value:Expr * args:Expr list -> Expr
        static member FieldGetUnchecked: pinfo:FieldInfo -> Expr
        static member FieldGetUnchecked: obj:Expr * pinfo:FieldInfo -> Expr
        static member FieldSetUnchecked: pinfo:FieldInfo * value:Expr -> Expr
        static member FieldSetUnchecked: obj:Expr * pinfo:FieldInfo * value:Expr -> Expr
        static member TupleGetUnchecked: e:Expr * n:int -> Expr
        static member LetUnchecked: v:Var * e:Expr * body:Expr -> Expr

      type Shape
      val ( |ShapeCombinationUnchecked|ShapeVarUnchecked|ShapeLambdaUnchecked| ): e:Expr -> Choice<(Shape * Expr list),Var, (Var * Expr)>
      val RebuildShapeCombinationUnchecked: Shape * args:Expr list -> Expr

