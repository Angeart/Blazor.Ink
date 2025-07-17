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

    #region Margin

    public LayoutValue Margin
    {
        get => YG.NodeStyleGetMargin(_yogaNode, YGEdge.YGEdgeAll);
        set => ApplyMarginValue(YGEdge.YGEdgeAll, value);
    }

    public LayoutValue MarginX
    {
        get => YG.NodeStyleGetMargin(_yogaNode, YGEdge.YGEdgeHorizontal);
        set => ApplyMarginValue(YGEdge.YGEdgeHorizontal, value);
    }

    public LayoutValue MarginY
    {
        get => YG.NodeStyleGetMargin(_yogaNode, YGEdge.YGEdgeVertical);
        set => ApplyMarginValue(YGEdge.YGEdgeVertical, value);
    }

    public LayoutValue MarginTop
    {
        get => YG.NodeStyleGetMargin(_yogaNode, YGEdge.YGEdgeTop);
        set => ApplyMarginValue(YGEdge.YGEdgeTop, value);
    }

    public LayoutValue MarginRight
    {
        get => YG.NodeStyleGetMargin(_yogaNode, YGEdge.YGEdgeRight);
        set => ApplyMarginValue(YGEdge.YGEdgeRight, value);
    }

    public LayoutValue MarginBottom
    {
        get => YG.NodeStyleGetMargin(_yogaNode, YGEdge.YGEdgeBottom);
        set => ApplyMarginValue(YGEdge.YGEdgeBottom, value);
    }

    public LayoutValue MarginLeft
    {
        get => YG.NodeStyleGetMargin(_yogaNode, YGEdge.YGEdgeLeft);
        set => ApplyMarginValue(YGEdge.YGEdgeLeft, value);
    }

    #endregion

    #region Padding

    public LayoutValue Padding
    {
        get => YG.NodeStyleGetPadding(_yogaNode, YGEdge.YGEdgeAll);
        set => ApplyPaddingValue(YGEdge.YGEdgeAll, value);
    }

    public LayoutValue PaddingX
    {
        get => YG.NodeStyleGetPadding(_yogaNode, YGEdge.YGEdgeHorizontal);
        set => ApplyPaddingValue(YGEdge.YGEdgeHorizontal, value);
    }

    public LayoutValue PaddingY
    {
        get => YG.NodeStyleGetPadding(_yogaNode, YGEdge.YGEdgeVertical);
        set => ApplyPaddingValue(YGEdge.YGEdgeVertical, value);
    }

    public LayoutValue PaddingTop
    {
        get => YG.NodeStyleGetPadding(_yogaNode, YGEdge.YGEdgeTop);
        set => ApplyPaddingValue(YGEdge.YGEdgeTop, value);
    }

    public LayoutValue PaddingRight
    {
        get => YG.NodeStyleGetPadding(_yogaNode, YGEdge.YGEdgeRight);
        set => ApplyPaddingValue(YGEdge.YGEdgeRight, value);
    }

    public LayoutValue PaddingBottom
    {
        get => YG.NodeStyleGetPadding(_yogaNode, YGEdge.YGEdgeBottom);
        set => ApplyPaddingValue(YGEdge.YGEdgeBottom, value);
    }

    public LayoutValue PaddingLeft
    {
        get => YG.NodeStyleGetPadding(_yogaNode, YGEdge.YGEdgeLeft);
        set => ApplyPaddingValue(YGEdge.YGEdgeLeft, value);
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

    public LayoutValue Position
    {
        get => YG.NodeStyleGetPosition(_yogaNode, YGEdge.YGEdgeAll);
        set => ApplyPosition(YGEdge.YGEdgeAll, value);
    }

    public LayoutValue PositionX
    {
        get => YG.NodeStyleGetPosition(_yogaNode, YGEdge.YGEdgeHorizontal);
        set => ApplyPosition(YGEdge.YGEdgeHorizontal, value);
    }

    public LayoutValue PositionY
    {
        get => YG.NodeStyleGetPosition(_yogaNode, YGEdge.YGEdgeVertical);
        set => ApplyPosition(YGEdge.YGEdgeVertical, value);
    }

    public LayoutValue PositionTop
    {
        get => YG.NodeStyleGetPosition(_yogaNode, YGEdge.YGEdgeTop);
        set => ApplyPosition(YGEdge.YGEdgeTop, value);
    }

    public LayoutValue PositionRight
    {
        get => YG.NodeStyleGetPosition(_yogaNode, YGEdge.YGEdgeRight);
        set => ApplyPosition(YGEdge.YGEdgeRight, value);
    }

    public LayoutValue PositionBottom
    {
        get => YG.NodeStyleGetPosition(_yogaNode, YGEdge.YGEdgeBottom);
        set => ApplyPosition(YGEdge.YGEdgeBottom, value);
    }

    public LayoutValue PositionLeft
    {
        get => YG.NodeStyleGetPosition(_yogaNode, YGEdge.YGEdgeLeft);
        set => ApplyPosition(YGEdge.YGEdgeLeft, value);
    }

    public LayoutPositionType PositionType
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

    public Align AlignItems
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
                    YG.NodeStyleSetFlexBasis(_yogaNode, value.Value);
                    break;
                case LayoutUnit.Percent:
                    YG.NodeStyleSetFlexBasisPercent(_yogaNode, value.Value);
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

    public LayoutWrap FlexWrap
    {
        get => YG.NodeStyleGetFlexWrap(_yogaNode).ToLayoutWrap();
        set => YG.NodeStyleSetFlexWrap(_yogaNode, value.ToYogaWrap());
    }

    #endregion

    #region Computed

    public ComputedProperties Computed { get; }

    public readonly struct ComputedProperties(YGNode* yogaNode)
    {
        public float Left => YG.NodeLayoutGetLeft(yogaNode);

        public float Top => YG.NodeLayoutGetTop(yogaNode);

        public float Right => YG.NodeLayoutGetRight(yogaNode);

        public float Bottom => YG.NodeLayoutGetBottom(yogaNode);

        public float Width => YG.NodeLayoutGetWidth(yogaNode);

        public float Height => YG.NodeLayoutGetHeight(yogaNode);

        public Direction Direction => YG.NodeLayoutGetDirection(yogaNode).ToLayoutDirection();

        public bool HadOverflow => YG.NodeLayoutGetHadOverflow(yogaNode) != 0;

        public (float Top, float Left, float Bottom, float Right) GetLayoutMargin()
        {
            return (
                YG.NodeLayoutGetMargin(yogaNode, YGEdge.YGEdgeTop),
                YG.NodeLayoutGetMargin(yogaNode, YGEdge.YGEdgeLeft),
                YG.NodeLayoutGetMargin(yogaNode, YGEdge.YGEdgeBottom),
                YG.NodeLayoutGetMargin(yogaNode, YGEdge.YGEdgeRight)
            );
        }

        public (float Top, float Left, float Bottom, float Right) GetLayoutBorder()
        {
            return (
                YG.NodeLayoutGetBorder(yogaNode, YGEdge.YGEdgeTop),
                YG.NodeLayoutGetBorder(yogaNode, YGEdge.YGEdgeLeft),
                YG.NodeLayoutGetBorder(yogaNode, YGEdge.YGEdgeBottom),
                YG.NodeLayoutGetBorder(yogaNode, YGEdge.YGEdgeRight)
            );
        }

        public (float Top, float Left, float Bottom, float Right) GetLayoutPadding()
        {
            return (
                YG.NodeLayoutGetPadding(yogaNode, YGEdge.YGEdgeTop),
                YG.NodeLayoutGetPadding(yogaNode, YGEdge.YGEdgeLeft),
                YG.NodeLayoutGetPadding(yogaNode, YGEdge.YGEdgeBottom),
                YG.NodeLayoutGetPadding(yogaNode, YGEdge.YGEdgeRight)
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
    
    public bool IsDirty =>  YG.NodeIsDirty(_yogaNode) != 0;

    #endregion

    #region Children Management

    public void InsertChild(LayoutNode child, nuint index)
    {
        YG.NodeInsertChild(_yogaNode, child._yogaNode, index);
        child.UpdateInheritance();
    }

    public void SwapChild(LayoutNode child, nuint index)
    {
        YG.NodeSwapChild(_yogaNode, child._yogaNode, index);
        child.UpdateInheritance();
    }

    public void RemoveChild(LayoutNode child)
    {
        YG.NodeRemoveChild(_yogaNode, child._yogaNode);
        child.UpdateInheritance();
    }

    public void RemoveAllChildren()
    {
        var children = new LayoutNode[GetChildCount()];
        for (var i = 0; i < children.Length; i++)
        {
            children[i] = GetChild(i)!.Value;
        }
        YG.NodeRemoveAllChildren(_yogaNode);
        foreach (var child in children)
        {
            child.UpdateInheritance();
        }
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
        {
            var context = YG.ConfigGetContext(YogaConfig);
            var ptr = (IntPtr)context;
            if (ptr == IntPtr.Zero)
            {
                return default;
            }

            var handle = GCHandle.FromIntPtr(ptr);
            var ctx = (TContext?)handle.Target;
            handle.Free();

            return ctx;
        }

        internal void SetContext<TContext>(TContext? context)
        {
            if (context == null)
            {
                YG.ConfigSetContext(YogaConfig, null);
                return;
            }

            _pinHandle?.Free();
            _pinHandle = GCHandle.Alloc(context, GCHandleType.Normal);
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
        Computed = new ComputedProperties(_yogaNode);
        _yogaNode = YG.NodeNew();
        _config = InitConfig();
        _config.SetContext(this);
        UseFastDispose = useFastDispose;
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

        return new Configuration();
    }

    private void ApplyConfig()
    {
        YG.NodeSetConfig(_yogaNode, Config.YogaConfig);
    }

    private void ApplyMarginValue(YGEdge edge, LayoutValue value)
    {
        ApplyLayoutValue(
            "Margin",
            _yogaNode,
            YG.NodeStyleSetMargin,
            YG.NodeStyleSetMarginPercent,
            YG.NodeStyleSetMarginAuto,
            null,
            null,
            edge, value);
    }

    private void ApplyPaddingValue(YGEdge edge, LayoutValue value)
    {
        ApplyLayoutValue(
            "Padding",
            _yogaNode,
            YG.NodeStyleSetPadding,
            YG.NodeStyleSetPaddingPercent,
            null,
            null,
            null,
            edge, value);
    }

    private void ApplyPosition(YGEdge edge, LayoutValue value)
    {
        ApplyLayoutValue(
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

    private static void ApplyLayoutValue(
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
                point?.Invoke(node, edge, value.Value);
                if (point is null)
                {
                    throw new ArgumentNullException(nameof(point), $"{name} has no setter for Point unit.");
                }

                break;
            case LayoutUnit.Percent:
                percent?.Invoke(node, edge, value.Value);
                if (percent is null)
                {
                    throw new ArgumentNullException(nameof(percent), $"{name} has no setter for Percent unit.");
                }

                break;
            case LayoutUnit.Auto:
                auto?.Invoke(node, edge);
                if (auto is null)
                {
                    throw new ArgumentNullException(nameof(auto), $"{name} has no setter for Auto unit.");
                }

                break;
            case LayoutUnit.MaxContent:
                maxContent?.Invoke(node, edge);
                if (maxContent is null)
                {
                    throw new ArgumentNullException(nameof(maxContent), $"{name} has no setter for MaxContent unit.");
                }

                break;
            case LayoutUnit.FitContent:
                fitContent?.Invoke(node, edge);
                if (fitContent is null)
                {
                    throw new ArgumentNullException(nameof(fitContent), $"{name} has no setter for FitContent unit.");
                }

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(value), "Invalid LayoutValue unit.");
        }
    }

    #endregion
}