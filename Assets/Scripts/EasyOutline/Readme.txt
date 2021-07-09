Instructions
------------

To add an outline to an object, drag-and-drop the Outline.cs script onto the
object. The outline materials will be loaded at runtime.

You can also add or set outlines programmatically with:

    var outline = gameObject.AddComponent<Outline>();

    outline.OutlineMode = Outline.Mode.OutlineAll;
    outline.OutlineColor = Color.yellow;
    outline.OutlineWidth = 5f;

Use outline.enabled to toggle the outline. Avoid removing and
re-adding the component if possible. Also try to add at scene starts since most process done in Awake().

For large meshes, you may also like to enable 'Precompute Outline' in the
editor. This will reduce the amount of work performed in Awake().