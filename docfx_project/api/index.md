## Examples

### Getting image dimensions and format

```c#
using Imageflow.Fluent;

public async void TestGetImageInfo()
{
    var imageBytes = Convert.FromBase64String(
        "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABAQMAAAAl21bKAAAAA1BMVEX/TQBcNTh/AAAAAXRSTlPM0jRW/QAAAApJREFUeJxjYgAAAAYAAzY3fKgAAAAASUVORK5CYII=");

    var info = await ImageJob.GetImageInfo(new BytesSource(imageBytes));

    Assert.Equal(info.ImageWidth, 1);
    Assert.Equal(info.ImageHeight, 1);
    Assert.Equal(info.PreferredExtension, "png");
    Assert.Equal(info.PreferredMimeType, "image/png");
}
```

### Edit images with the fluent API

```c#
using Imageflow.Fluent;
public async Task TestAllJob()
{
    var imageBytes = Convert.FromBase64String(
        "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABAQMAAAAl21bKAAAAA1BMVEX/TQBcNTh/AAAAAXRSTlPM0jRW/QAAAApJREFUeJxjYgAAAAYAAzY3fKgAAAAASUVORK5CYII=");
    using (var b = new ImageJob())
    {
        var r = await b.Decode(imageBytes)
            .FlipVertical()
            .FlipHorizontal()
            .Rotate90()
            .Rotate180()
            .Rotate270()
            .Transpose()
            .CropWhitespace(80, 0.5f)
            .Distort(30, 20)
            .Crop(0,0,10,10)
            .Region(-5,-5,10,10, AnyColor.Black)
            .RegionPercent(-10f, -10f, 110f, 110f, AnyColor.Transparent)
            .BrightnessSrgb(-1f)
            .ContrastSrgb(1f)
            .SaturationSrgb(1f)
            .WhiteBalanceSrgb(80)
            .ColorFilterSrgb(ColorFilterSrgb.Invert)
            .ColorFilterSrgb(ColorFilterSrgb.Sepia)
            .ColorFilterSrgb(ColorFilterSrgb.Grayscale_Bt709)
            .ColorFilterSrgb(ColorFilterSrgb.Grayscale_Flat)
            .ColorFilterSrgb(ColorFilterSrgb.Grayscale_Ntsc)
            .ColorFilterSrgb(ColorFilterSrgb.Grayscale_Ry)
            .ExpandCanvas(5,5,5,5,AnyColor.FromHexSrgb("FFEECCFF"))
            .FillRectangle(2,2,8,8, AnyColor.Black)
            .ResizerCommands("width=10&height=10&mode=crop")
            .ConstrainWithin(5, 5)
            .Watermark(new BytesSource(imageBytes),
                new WatermarkOptions()
                   .SetMarginsLayout(
                        new WatermarkMargins(WatermarkAlign.Image, 1,1,1,1),
                        WatermarkConstraintMode.Within,
                        new ConstraintGravity(90,90))
                    .SetOpacity(0.5f)
                    .SetHints(new ResampleHints().SetSharpen(15f, SharpenWhen.Always))
                    .SetMinCanvasSize(1,1))
            .EncodeToBytes(new MozJpegEncoder(80,true))
            .Finish().InProcessAsync();

        Assert.Equal(5, r.First.Width);
        Assert.True(r.First.TryGetBytes().HasValue);
    }
}
```

### Generate multiple versions of one image

```c#
using Imageflow.Fluent;
public async Task TestMultipleOutputs()
{
    var imageBytes = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABAQMAAAAl21bKAAAAA1BMVEX/TQBcNTh/AAAAAXRSTlPM0jRW/QAAAApJREFUeJxjYgAAAAYAAzY3fKgAAAAASUVORK5CYII=");
    using (var b = new ImageJob())
    {
        var r = await b.Decode(imageBytes).
            Constrain(new Constraint(ConstraintMode.Fit, 160, 120))
            .Branch(f => f.ConstrainWithin(80, 60).EncodeToBytes(new WebPLosslessEncoder()))
            .Branch(f => f.ConstrainWithin(40, 30).EncodeToBytes(new WebPLossyEncoder(50)))
            .EncodeToBytes(new LodePngEncoder())
            .Finish().InProcessAsync();

        Assert.Equal(60, r.TryGet(1).Width);
        Assert.Equal(30, r.TryGet(2).Width);
        Assert.Equal(120, r.TryGet(3).Width);
        Assert.True(r.First.TryGetBytes().HasValue);
    }

}
```

### Edit images using a [command string](https://docs.imageflow.io/querystring/introduction.html)

```c#
using Imageflow.Fluent;

public async Task TestBuildCommandString()
{
    var imageBytes = Convert.FromBase64String(
        "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABAQMAAAAl21bKAAAAA1BMVEX/TQBcNTh/AAAAAXRSTlPM0jRW/QAAAApJREFUeJxjYgAAAAYAAzY3fKgAAAAASUVORK5CYII=");
    // We wrap the job in a using() statement to free memory faster
    using (var b = new ImageJob())
    {

        var r = await b.BuildCommandString(
            new BytesSource(imageBytes), // or new StreamSource(Stream stream, bool disposeStream)
            new BytesDestination(), // or new StreamDestination
            "width=3&height=2&mode=stretch&scale=both&format=webp&webp.quality=80")
            .Finish().InProcessAsync();

        Assert.Equal(3, r.First.Width);
        Assert.Equal("webp", r.First.PreferredExtension);
        Assert.True(r.First.TryGetBytes().HasValue);
    }
}

```

## [More examples are in the tests](https://github.com/imazen/imageflow-dotnet/blob/master/tests/Imageflow.Test/TestApi.cs)

- [Project source and issue site](https://github.com/imazen/imageflow-dotnet)
