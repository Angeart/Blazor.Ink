using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Blazor.Ink.Core.Layout.Value;
using Blazor.Ink.Core.Layout.Value.Config;
using Yoga;

namespace Blazor.Ink.Core.Layout;

public unsafe struct LayoutNode : IDisposable
{
    private readonly YGNode* _yogaNode;

    /// <summary>
    /// When set to true, the node will dispose without unregistering child trees;.
    /// </summary>
    public bool UseFastDispose { get; set; } = false;

    #region Basic

    public Overflow Overflow
    {
        get => YG.NodeStyleGetOverflow(_yogaNode).ToLayoutOverflow();
        set => YG.NodeStyleSetOverflow(_yogaNode, value.ToYogaOverflow());
    }

    public Display Display
    {
        get => YG.NodeStyleGetDisplay(_yogaNode).ToLayoutDisplay();
        set => YG.NodeStyleSetDisplay(_yogaNode, value.ToYogaDisplay());
    }

    #endregion

    #region Size

    public LayoutValue Width
    {
        get => YG.NodeStyleGetWidth(_yogaNode);
        set
        {
            switch (value.Unit)
            {
                case LayoutUnit.Point:
                    YG.NodeStyleSetWidth(_yogaNode, value.ValueAsFloat);
                    break;
                case LayoutUnit.Percent:
                    YG.NodeStyleSetWidthPercent(_yogaNode, value.ValueAsFloat);
                    break;
                case LayoutUnit.Auto:
                    YG.NodeStyleSetWidthAuto(_yogaNode);
                    break;
                case LayoutUnit.MaxContent:
                    YG.NodeStyleSetWidthMaxContent(_yogaNode);
                    break;
                case LayoutUnit.FitContent:
                    YG.NodeStyleSetWidthFitContent(_yogaNode);
                    break;
                case LayoutUnit.Stretch:
                    YG.NodeStyleSetWidthStretch(_yogaNode);
                    break;
                default:
                    ThrowArgumentOutOfRangeException($"Invalid LayoutValue unit: {value.Unit}");
                    break;
            }
        }
    }

    public LayoutValue Height
    {
        get => YG.NodeStyleGetHeight(_yogaNode);
        set
        {
            switch (value.Unit)
            {
                case LayoutUnit.Point:
                    YG.NodeStyleSetHeight(_yogaNode, value.ValueAsFloat);
                    break;
                case LayoutUnit.Percent:
                    YG.NodeStyleSetHeightPercent(_yogaNode, value.ValueAsFloat);
                    break;
                case LayoutUnit.Auto:
                    YG.NodeStyleSetHeightAuto(_yogaNode);
                    break;
                case LayoutUnit.MaxContent:
                    YG.NodeStyleSetHeightMaxContent(_yogaNode);
                    break;
                case LayoutUnit.FitContent:
                    YG.NodeStyleSetHeightFitContent(_yogaNode);
                    break;
                case LayoutUnit.Stretch:
                    YG.NodeStyleSetHeightStretch(_yogaNode);
                    break;
                default:
                    ThrowArgumentOutOfRangeException($"Invalid LayoutValue unit: {value.Unit}");
                    break;
            }
        }
    }

    public float AspectRatio
    {
        get => YG.NodeStyleGetAspectRatio(_yogaNode);
        set => YG.NodeStyleSetAspectRatio(_yogaNode, value);
    }

    #endregion

    #region Margin

    public MarginValue Margin
    {
        get => YG.NodeStyleGetMargin(_yogaNode, YGEdge.YGEdgeAll);
        set => ApplyMargin(YGEdge.YGEdgeAll, value);
    }

    public MarginValue MarginX
    {
        get => YG.NodeStyleGetMargin(_yogaNode, YGEdge.YGEdgeHorizontal);
        set => ApplyMargin(YGEdge.YGEdgeHorizontal, value);
    }

    public MarginValue MarginY
    {
        get => YG.NodeStyleGetMargin(_yogaNode, YGEdge.YGEdgeVertical);
        set => ApplyMargin(YGEdge.YGEdgeVertical, value);
    }

    public MarginValue MarginTop
    {
        get => YG.NodeStyleGetMargin(_yogaNode, YGEdge.YGEdgeTop);
        set => ApplyMargin(YGEdge.YGEdgeTop, value);
    }

    public MarginValue MarginRight
    {
        get => YG.NodeStyleGetMargin(_yogaNode, YGEdge.YGEdgeRight);
        set => ApplyMargin(YGEdge.YGEdgeRight, value);
    }

    public MarginValue MarginBottom
    {
        get => YG.NodeStyleGetMargin(_yogaNode, YGEdge.YGEdgeBottom);
        set => ApplyMargin(YGEdge.YGEdgeBottom, value);
    }

    public MarginValue MarginLeft
    {
        get => YG.NodeStyleGetMargin(_yogaNode, YGEdge.YGEdgeLeft);
        set => ApplyMargin(YGEdge.YGEdgeLeft, value);
    }

    #endregion

    #region Padding

    public PaddingValue Padding
    {
        get => YG.NodeStyleGetPadding(_yogaNode, YGEdge.YGEdgeAll);
        set => ApplyPadding(YGEdge.YGEdgeAll, value);
    }

    public PaddingValue PaddingX
    {
        get => YG.NodeStyleGetPadding(_yogaNode, YGEdge.YGEdgeHorizontal);
        set => ApplyPadding(YGEdge.YGEdgeHorizontal, value);
    }

    public PaddingValue PaddingY
    {
        get => YG.NodeStyleGetPadding(_yogaNode, YGEdge.YGEdgeVertical);
        set => ApplyPadding(YGEdge.YGEdgeVertical, value);
    }

    public PaddingValue PaddingTop
    {
        get => YG.NodeStyleGetPadding(_yogaNode, YGEdge.YGEdgeTop);
        set => ApplyPadding(YGEdge.YGEdgeTop, value);
    }

    public PaddingValue PaddingRight
    {
        get => YG.NodeStyleGetPadding(_yogaNode, YGEdge.YGEdgeRight);
        set => ApplyPadding(YGEdge.YGEdgeRight, value);
    }

    public PaddingValue PaddingBottom
    {
        get => YG.NodeStyleGetPadding(_yogaNode, YGEdge.YGEdgeBottom);
        set => ApplyPadding(YGEdge.YGEdgeBottom, value);
    }

    public PaddingValue PaddingLeft
    {
        get => YG.NodeStyleGetPadding(_yogaNode, YGEdge.YGEdgeLeft);
        set => ApplyPadding(YGEdge.YGEdgeLeft, value);
    }

    #endregion

    #region Border

    public float Border
    {
        get => YG.NodeStyleGetBorder(_yogaNode, YGEdge.YGEdgeAll);
        set => YG.NodeStyleSetBorder(_yogaNode, YGEdge.YGEdgeAll, value);
    }

    public float BorderX
    {
        get => YG.NodeStyleGetBorder(_yogaNode, YGEdge.YGEdgeHorizontal);
        set => YG.NodeStyleSetBorder(_yogaNode, YGEdge.YGEdgeHorizontal, value);
    }

    public float BorderY
    {
        get => YG.NodeStyleGetBorder(_yogaNode, YGEdge.YGEdgeVertical);
        set => YG.NodeStyleSetBorder(_yogaNode, YGEdge.YGEdgeVertical, value);
    }

    public float BorderTop
    {
        get => YG.NodeStyleGetBorder(_yogaNode, YGEdge.YGEdgeTop);
        set => YG.NodeStyleSetBorder(_yogaNode, YGEdge.YGEdgeTop, value);
    }

    public float BorderRight
    {
        get => YG.NodeStyleGetBorder(_yogaNode, YGEdge.YGEdgeRight);
        set => YG.NodeStyleSetBorder(_yogaNode, YGEdge.YGEdgeRight, value);
    }

    public float BorderBottom
    {
        get => YG.NodeStyleGetBorder(_yogaNode, YGEdge.YGEdgeBottom);
        set => YG.NodeStyleSetBorder(_yogaNode, YGEdge.YGEdgeBottom, value);
    }

    public float BorderLeft
    {
        get => YG.NodeStyleGetBorder(_yogaNode, YGEdge.YGEdgeLeft);
        set => YG.NodeStyleSetBorder(_yogaNode, YGEdge.YGEdgeLeft, value);
    }

    #endregion

    #region Position

    public PositionValue Top
    {
        get => YG.NodeStyleGetPosition(_yogaNode, YGEdge.YGEdgeTop);
        set => ApplyPosition(YGEdge.YGEdgeTop, value);
    }

    public PositionValue Right
    {
        get => YG.NodeStyleGetPosition(_yogaNode, YGEdge.YGEdgeRight);
        set => ApplyPosition(YGEdge.YGEdgeRight, value);
    }

    public PositionValue Bottom
    {
        get => YG.NodeStyleGetPosition(_yogaNode, YGEdge.YGEdgeBottom);
        set => ApplyPosition(YGEdge.YGEdgeBottom, value);
    }

    public PositionValue Left
    {
        get => YG.NodeStyleGetPosition(_yogaNode, YGEdge.YGEdgeLeft);
        set => ApplyPosition(YGEdge.YGEdgeLeft, value);
    }

    public PositionType Position
    {
        get => YG.NodeStyleGetPositionType(_yogaNode).ToLayoutPositionType();
        set => YG.NodeStyleSetPositionType(_yogaNode, value.ToYogaPositionType());
    }

    #endregion

    #region Flex

    public Direction Direction
    {
        get => YG.NodeStyleGetDirection(_yogaNode).ToLayoutDirection();
        set => YG.NodeStyleSetDirection(_yogaNode, value.ToYogaDirection());
    }

    public FlexDirection FlexDirection
    {
        get => YG.NodeStyleGetFlexDirection(_yogaNode).ToLayoutFlexDirection();
        set => YG.NodeStyleSetFlexDirection(_yogaNode, value.ToYogaFlexDirection());
    }

    public Justify JustifyContent
    {
        get => YG.NodeStyleGetJustifyContent(_yogaNode).ToLayoutJustify();
        set => YG.NodeStyleSetJustifyContent(_yogaNode, value.ToYogaJustify());
    }

    public Align AlignContent
    {
        get => YG.NodeStyleGetAlignContent(_yogaNode).ToLayoutAlign();
        set => YG.NodeStyleSetAlignContent(_yogaNode, value.ToYogaAlign());
    }

    public Align Align
    {
        get => YG.NodeStyleGetAlignItems(_yogaNode).ToLayoutAlign();
        set => YG.NodeStyleSetAlignItems(_yogaNode, value.ToYogaAlign());
    }

    public Align AlignSelf
    {
        get => YG.NodeStyleGetAlignSelf(_yogaNode).ToLayoutAlign();
        set => YG.NodeStyleSetAlignSelf(_yogaNode, value.ToYogaAlign());
    }

    public float? Flex
    {
        get
        {
            var value = YG.NodeStyleGetFlex(_yogaNode);
            if (float.IsNaN(value))
            {
                return null;
            }

            return value;
        }
        set => YG.NodeStyleSetFlex(_yogaNode, value ?? YG.YGUndefined);
    }

    public float FlexGrow
    {
        get => YG.NodeStyleGetFlexGrow(_yogaNode);
        set => YG.NodeStyleSetFlexGrow(_yogaNode, value);
    }

    public float FlexShrink
    {
        get => YG.NodeStyleGetFlexShrink(_yogaNode);
        set => YG.NodeStyleSetFlexShrink(_yogaNode, value);
    }

    public LayoutValue FlexBasis
    {
        get => YG.NodeStyleGetFlexBasis(_yogaNode);
        set
        {
            switch (value.Unit)
            {
                case LayoutUnit.Point:
                    YG.NodeStyleSetFlexBasis(_yogaNode, value.ValueAsFloat);
                    break;
                case LayoutUnit.Percent:
                    YG.NodeStyleSetFlexBasisPercent(_yogaNode, value.ValueAsFloat);
                    break;
                case LayoutUnit.Auto:
                    YG.NodeStyleSetFlexBasisAuto(_yogaNode);
                    break;
                case LayoutUnit.MaxContent:
                    YG.NodeStyleSetFlexBasisMaxContent(_yogaNode);
                    break;
                case LayoutUnit.FitContent:
                    YG.NodeStyleSetFlexBasisFitContent(_yogaNode);
                    break;
                case LayoutUnit.Stretch:
                    YG.NodeStyleSetFlexBasisStretch(_yogaNode);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), "Invalid LayoutValue unit for FlexBasis.");
            }
        }
    }

    public FlexWrap FlexWrap
    {
        get => YG.NodeStyleGetFlexWrap(_yogaNode).ToLayoutWrap();
        set => YG.NodeStyleSetFlexWrap(_yogaNode, value.ToYogaWrap());
    }

    #endregion

    #region Computed

    public ComputedProperties Computed { get; }

    public readonly struct ComputedProperties(YGNode* yogaNode)
    {
        public int Left => (int)YG.NodeLayoutGetLeft(yogaNode);

        public int Top => (int)YG.NodeLayoutGetTop(yogaNode);

        public int Right => (int)YG.NodeLayoutGetRight(yogaNode);

        public int Bottom => (int)YG.NodeLayoutGetBottom(yogaNode);

        public int Width => (int)YG.NodeLayoutGetWidth(yogaNode);

        public int Height => (int)YG.NodeLayoutGetHeight(yogaNode);

        public Direction Direction => YG.NodeLayoutGetDirection(yogaNode).ToLayoutDirection();

        public bool HadOverflow => YG.NodeLayoutGetHadOverflow(yogaNode) != 0;

        public (int Top, int Left, int Bottom, int Right) GetLayoutMargin()
        {
            return (
                (int)YG.NodeLayoutGetMargin(yogaNode, YGEdge.YGEdgeTop),
                (int)YG.NodeLayoutGetMargin(yogaNode, YGEdge.YGEdgeLeft),
                (int)YG.NodeLayoutGetMargin(yogaNode, YGEdge.YGEdgeBottom),
                (int)YG.NodeLayoutGetMargin(yogaNode, YGEdge.YGEdgeRight)
            );
        }

        public (int Top, int Left, int Bottom, int Right) GetLayoutBorder()
        {
            return (
                (int)YG.NodeLayoutGetBorder(yogaNode, YGEdge.YGEdgeTop),
                (int)YG.NodeLayoutGetBorder(yogaNode, YGEdge.YGEdgeLeft),
                (int)YG.NodeLayoutGetBorder(yogaNode, YGEdge.YGEdgeBottom),
                (int)YG.NodeLayoutGetBorder(yogaNode, YGEdge.YGEdgeRight)
            );
        }

        public (int Top, int Left, int Bottom, int Right) GetLayoutPadding()
        {
            return (
                (int)YG.NodeLayoutGetPadding(yogaNode, YGEdge.YGEdgeTop),
                (int)YG.NodeLayoutGetPadding(yogaNode, YGEdge.YGEdgeLeft),
                (int)YG.NodeLayoutGetPadding(yogaNode, YGEdge.YGEdgeBottom),
                (int)YG.NodeLayoutGetPadding(yogaNode, YGEdge.YGEdgeRight)
            );
        }
    }

    #endregion

    #region Methods

    public void Reset()
    {
        YG.NodeReset(_yogaNode);
    }

    public void CalculateLayout(float availableWidth, float availableHeight, YGDirection ownerDirection)
    {
        YG.NodeCalculateLayout(_yogaNode, availableWidth, availableHeight, ownerDirection);
    }

    public void CopyStyleFrom(LayoutNode src)
    {
        YG.NodeCopyStyle(_yogaNode, src._yogaNode);
    }

    public void CopyStyleTo(LayoutNode dest)
    {
        YG.NodeCopyStyle(dest._yogaNode, _yogaNode);
    }

    public void MarkDirty()
    {
        YG.NodeMarkDirty(_yogaNode);
    }

    public bool IsDirty => YG.NodeIsDirty(_yogaNode) != 0;

    #endregion

    #region Children Management

    public void InsertChild(LayoutNode child, int index)
    {
        YG.NodeInsertChild(_yogaNode, child._yogaNode, (nuint)index);
        child.UpdateInheritance();
    }

    public void SwapChild(LayoutNode child, int index)
    {
        YG.NodeSwapChild(_yogaNode, child._yogaNode, (nuint)index);
        child.UpdateInheritance();
    }

    public void RemoveChild(LayoutNode child)
    {
        YG.NodeRemoveChild(_yogaNode, child._yogaNode);
        child.UpdateInheritance();
    }

    public void RemoveAllChildren()
    {
        var childCount = GetChildCount();
        var children = ArrayPool<LayoutNode>.Shared.Rent(childCount);
        for (var i = 0; i < childCount; i++)
        {
            children[i] = GetChild(i)!.Value;
        }

        YG.NodeRemoveAllChildren(_yogaNode);
        foreach (var child in children.AsSpan(0, childCount))
        {
            child.UpdateInheritance();
        }

        ArrayPool<LayoutNode>.Shared.Return(children);
    }

    public void SetChildren(LayoutNode[] children)
    {
        var nodes = new YGNode*[children.Length];
        for (var i = 0; i < children.Length; i++)
            nodes[i] = children[i]._yogaNode;
        fixed (YGNode** ptr = nodes)
        {
            YG.NodeSetChildren(_yogaNode, ptr, (nuint)children.Length);
        }

        foreach (var child in children)
        {
            child.UpdateInheritance();
        }
    }

    public LayoutNode? GetChild(int index)
    {
        var node = YG.NodeGetChild(_yogaNode, (nuint)index);
        return GetLayoutNodeFromContext(node);
    }

    public int GetChildCount()
    {
        var count = YG.NodeGetChildCount(_yogaNode);
        if (count > int.MaxValue)
        {
            throw new InvalidOperationException("Child count exceeds maximum integer value.");
        }

        return (int)count;
    }

    // NOTE: Currently GetOwner is not implemented because it's not needed.

    public LayoutNode? GetParent()
    {
        var parentPtr = YG.NodeGetParent(_yogaNode);
        return GetLayoutNodeFromContext(parentPtr);
    }

    #endregion

    #region Configuration

    private Configuration _config;
    private bool _useInheritedConfig = true;

    public bool UseInheritedConfig
    {
        get => _useInheritedConfig;
        set
        {
            if (_useInheritedConfig == value) return;
            _useInheritedConfig = value;
            UpdateInheritance();
        }
    }

    public Configuration Config => _config;

    public struct Configuration : IDisposable
    {
        public static Configuration Empty => new();
        public static Configuration WebDefaults => new(true);
        internal readonly YGConfig* YogaConfig;
        private GCHandle? _pinHandle;

        public bool Inherited { get; private set; }

        public bool UseWebDefaults
        {
            get => YG.ConfigGetUseWebDefaults(YogaConfig) != 0;
            set => YG.ConfigSetUseWebDefaults(YogaConfig, (byte)(value ? 1 : 0));
        }

        public float PointScaleFactor
        {
            get => YG.ConfigGetPointScaleFactor(YogaConfig);
            set => YG.ConfigSetPointScaleFactor(YogaConfig, value);
        }

        public Errata Errata
        {
            get => YG.ConfigGetErrata(YogaConfig).ToLayoutConfigErrata();
            set => YG.ConfigSetErrata(YogaConfig, value.ToYogaErrata());
        }

        // NOTE: Currently SetLogger is not implemented because it's not needed.
        // NOTE: Currently SetExperimentalFeatureEnabled is not implemented because it's not needed.

        internal TContext? GetContext<TContext>()
        where TContext : struct
        {
            var context = YG.ConfigGetContext(YogaConfig);
            var ptr = (IntPtr)context;
            if (ptr == IntPtr.Zero)
            {
                return null;
            }

            var handle = GCHandle.FromIntPtr(ptr);
            var ctx = (TContext?)handle.Target;

            return ctx;
        }

        internal void SetContext<TContext>(TContext? context)
        where TContext : struct
        {
            if (context == null)
            {
                YG.ConfigSetContext(YogaConfig, null);
                return;
            }

            _pinHandle?.Free();
            _pinHandle = GCHandle.Alloc(context.Value, GCHandleType.Pinned);
            var ptr = GCHandle.ToIntPtr(_pinHandle.Value);
            YG.ConfigSetContext(YogaConfig, (void*)ptr);
        }

        public Configuration()
        {
            YogaConfig = YG.ConfigNew();
            if (YogaConfig == null)
            {
                throw new InvalidOperationException("Failed to create Yoga configuration.");
            }

            Errata = Errata.Classic;
        }

        private Configuration(bool useWebDefaults) : this()
        {
            UseWebDefaults = useWebDefaults;
        }

        internal Configuration(YGConfig* yogaConfig)
        {
            if (yogaConfig == null)
            {
                throw new ArgumentNullException(nameof(yogaConfig), "YogaConfig cannot be null.");
            }

            YogaConfig = yogaConfig;
        }

        public static Configuration InheritFrom(Configuration parent)
        {
            if (parent.YogaConfig == null)
            {
                throw new ArgumentNullException(nameof(parent), "Cannot inherit from a null YogaConfig.");
            }

            var config = new Configuration(parent.YogaConfig);
            config.Inherited = true;

            return config;
        }

        public void Dispose()
        {
            if (Inherited) return;
            YG.ConfigFree(YogaConfig);
            if (_pinHandle.HasValue)
            {
                _pinHandle.Value.Free();
                _pinHandle = null;
            }
        }
    }

    #endregion

    public LayoutNode(bool useFastDispose = false)
    {
        _yogaNode = YG.NodeNew();
        // NOTE: Should create after _yogaNode is initialized.
        Computed = new ComputedProperties(_yogaNode);
        _config = InitConfig();
        _config.SetContext<LayoutNode>(this);
        UseFastDispose = useFastDispose;
        ApplyConfig();
    }

    public LayoutNode() : this(false)
    {
        // Default constructor with fast dispose disabled.
    }

    public void Dispose()
    {
        if (UseFastDispose)
        {
            YG.NodeFinalize(_yogaNode);
        }
        else
        {
            YG.NodeFree(_yogaNode);
        }

        _config.Dispose();
    }

    #region Internals

    private static LayoutNode? GetLayoutNodeFromContext(YGNode* node)
    {
        if (node == null)
        {
            return null;
        }

        var ygConfig = YG.NodeGetConfig(node);
        if (ygConfig == null)
        {
            return null;
        }

        var config = new Configuration(ygConfig);
        return config.GetContext<LayoutNode>();
    }

    private void UpdateInheritance()
    {
        if (UseInheritedConfig)
        {
            _config = InitConfig();
            ApplyConfig();
        }
    }

    private Configuration InitConfig()
    {
        if (UseInheritedConfig)
        {
            var parent = GetParent();
            if (parent is null)
            {
                return GetNativeOrCreateConfig();
            }

            return Configuration.InheritFrom(parent.Value.Config);
        }

        return GetNativeOrCreateConfig();
    }

    private Configuration GetNativeOrCreateConfig()
    {
        var nodeConfig = YG.NodeGetConfig(_yogaNode);
        if (nodeConfig == null)
        {
            return Configuration.Empty;
        }

        var config = new Configuration();
        config.SetContext<LayoutNode>(this);
        return config;
    }

    private void ApplyConfig()
    {
        YG.NodeSetConfig(_yogaNode, Config.YogaConfig);
    }

    private void ApplyMargin(YGEdge edge, MarginValue value)
    {
        ApplyEdgeValue(
            "Margin",
            _yogaNode,
            YG.NodeStyleSetMargin,
            YG.NodeStyleSetMarginPercent,
            YG.NodeStyleSetMarginAuto,
            null,
            null,
            edge, value);
    }

    private void ApplyPadding(YGEdge edge, PaddingValue value)
    {
        ApplyEdgeValue(
            "Padding",
            _yogaNode,
            YG.NodeStyleSetPadding,
            YG.NodeStyleSetPaddingPercent,
            null,
            null,
            null,
            edge, value);
    }

    private void ApplyPosition(YGEdge edge, PositionValue value)
    {
        ApplyEdgeValue(
            "Position",
            _yogaNode,
            YG.NodeStyleSetPosition,
            YG.NodeStyleSetPositionPercent,
            YG.NodeStyleSetPositionAuto,
            null,
            null,
            edge, value);
    }

    private delegate void EdgeValueApplier(YGNode* node, YGEdge edge, float value);

    private delegate void EdgeApplier(YGNode* node, YGEdge edge);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ApplyEdgeValue(
        string name,
        YGNode* node,
        EdgeValueApplier? point,
        EdgeValueApplier? percent,
        EdgeApplier? auto,
        EdgeApplier? maxContent,
        EdgeApplier? fitContent,
        YGEdge edge,
        LayoutValue value)
    {
        switch (value.Unit)
        {
            case LayoutUnit.Point:
                point?.Invoke(node, edge, value.ValueAsFloat);
                if (point is null)
                {
                    ThrowArgumentNullException($"{name} has no setter for Point unit.", nameof(point));
                }

                break;
            case LayoutUnit.Percent:
                percent?.Invoke(node, edge, value.ValueAsFloat);
                if (percent is null)
                {
                    ThrowArgumentNullException($"{name} has no setter for Percent unit.", nameof(percent));
                }

                break;
            case LayoutUnit.Auto:
                auto?.Invoke(node, edge);
                if (auto is null)
                {
                    ThrowArgumentNullException($"{name} has no setter for Auto unit.", nameof(auto));
                }

                break;
            case LayoutUnit.MaxContent:
                maxContent?.Invoke(node, edge);
                if (maxContent is null)
                {
                    ThrowArgumentNullException($"{name} has no setter for MaxContent unit.", nameof(maxContent));
                }

                break;
            case LayoutUnit.FitContent:
                fitContent?.Invoke(node, edge);
                if (fitContent is null)
                {
                    ThrowArgumentNullException($"{name} has no setter for FitContent unit.", nameof(fitContent));
                }

                break;
            default:
                ThrowArgumentOutOfRangeException(
                    $"Invalid LayoutValue unit: {value.Unit}.",
                    nameof(value));
                break;
        }
    }

    #endregion

    #region Exceptions

    [DoesNotReturn]
    private static void ThrowArgumentNullException(string message, [CallerMemberName] string paramName = "")
    {
        throw new ArgumentNullException(paramName, message);
    }

    [DoesNotReturn]
    private static void ThrowArgumentOutOfRangeException(string message, [CallerMemberName] string paramName = "")
    {
        throw new ArgumentOutOfRangeException(paramName, message);
    }

    #endregion
}