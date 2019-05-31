Now, instead of using Cognitive Services' REST API, we'll add a previously trained model to the project for local evaluation with Windows ML.

## 1. Detect emotions with a local model and Windows ML

1. Download the ONNX **v1.2** model from the <a href="https://github.com/onnx/models/tree/master/emotion_ferplus">ONNX Model Zoo</a> and save it as `FER_Emotion_Recognition.onnx`.

2. In Visual Studio, drag and drop the downloaded `FER_Emotion_Recognition.onnx` file to the **Assets** folder in your Solution Explorer. Visual Studio will generate a new `FER_Emotion_Recognition.cs` file with the necessary code to create and execute the model.

3. Right click on the `FER_Emotion_Recognition.onnx` file, select **Properties**, set **Build Action** to "Content" and **Copy to Output Directory** to "Copy if newer".

4. In `MainPage.xaml.cs`, add the last global variable for the model.

    ```csharp
    private FER_Emotion_RecognitionModel model = new FER_Emotion_RecognitionModel();
    ```

5. Add the method below to initialize the model.

    ```csharp
    private async void InitializeModel()
    {
        string modelPath = @"ms-appx:///Assets/FER_Emotion_Recognition.onnx";
        StorageFile modelFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(modelPath));
        model = await FER_Emotion_RecognitionModel.CreateFromStreamAsync(modelFile);
    }
    ```

6. Call InitializeModel at the end of the MainPage constructor.

    ```csharp
    public MainPage()
    {
        this.InitializeComponent();
        this.InitializeModel();
    }
    ```

7. Create a new DetectEmotion method using the local model instead of the Cognitive Services API. Note that, in this case, we can use the VideoFrame from the camera directly.

    ```csharp
    private async Task<string> DetectEmotionWithWinML()
    {
        var videoFrame  = lastFrame;
        var input = ImageFeatureValue.CreateFromVideoFrame(videoFrame);
        var emotion = await model.EvaluateAsync(new FER_Emotion_RecognitionInput() { Input3 = input });
        var list = new List<float>(emotion.Plus692_Output_0.GetAsVectorView());
        var index = list.IndexOf(list.Max());
        var label = labels[index];
    
        return label;
    }
    ```

8. In the AnalyzeFrame method, replace the call to the previous DetectEmotion with this new one:
    
    ```csharp
    detectedEmotion = await DetectEmotionWithWinML();
    //detectedEmotion = await DetectEmotionWithCognitiveServices();
    ```

    Try running the application again!

## 2. Crop the image

In order to improve the detection of the emotion, the Cognitive Services Face API performs an automatic crop. Let's do the same manually for this scenario.

1. First, add the following global variable:

    ```csharp
    private FaceDetector faceDetector;
    ```
    
2. Then, add the following code at the beginning of the DetectEmotionWithML method.

    ```csharp
    var videoFrame = lastFrame;
    
    if (faceDetector == null)
    {
        faceDetector = await FaceDetector.CreateAsync();
    }
    
    var detectedFaces = await faceDetector.DetectFacesAsync(videoFrame.SoftwareBitmap);
    
    if (detectedFaces != null && detectedFaces.Any())
    {
        var face = detectedFaces.OrderByDescending(s => s.FaceBox.Height * s.FaceBox.Width).First();
        using (var randomAccessStream = new InMemoryRandomAccessStream())
        {
            var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.BmpEncoderId, randomAccessStream);
            var softwareBitmap = SoftwareBitmap.Convert(videoFrame.SoftwareBitmap, BitmapPixelFormat.Rgba16);
            Debug.WriteLine(softwareBitmap.BitmapPixelFormat);
            encoder.SetSoftwareBitmap(softwareBitmap);
            encoder.BitmapTransform.Bounds = new BitmapBounds
            {
                X = face.FaceBox.X,
                Y = face.FaceBox.Y,
                Width = face.FaceBox.Width,
                Height = face.FaceBox.Height
            };

            await encoder.FlushAsync();

            var decoder = await BitmapDecoder.CreateAsync(randomAccessStream);
            var croppedImage = await decoder.GetSoftwareBitmapAsync(softwareBitmap.BitmapPixelFormat, softwareBitmap.BitmapAlphaMode);

            videoFrame = VideoFrame.CreateWithSoftwareBitmap(croppedImage);
        }
    }    
    ```
    
    Now try the application again, and the results should improve!
    
## 3. Make the clock work

Sections 3 to 5 are just a little of bells and whistles to make the application look like an actual alarm clock. If you prefer to skip them, you can go directly to the next project, [Product detection with Custom Vision](#product-detection-with-custom-vision).

Let's add the code to update the clock.

1. In `MainPage.xaml`, add the following controls before the closing `</Grid>` tag.

    ```xaml
        <TextBlock Grid.Row="1" Grid.ColumnSpan="2" x:Name="EmotionText" VerticalAlignment="Center" HorizontalAlignment="Center" Foreground="White" FontSize="30"></TextBlock>
        <TextBlock Grid.Row="3" Grid.Column="0" x:Name="TimeText" VerticalAlignment="Center" HorizontalAlignment="Center" TextAlignment="Center" Margin="100,0,0,0"  Foreground="White" FontSize="200">12:00:00</TextBlock>
        <TextBlock Grid.Row="3" Grid.Column="1" x:Name="AlarmText" VerticalAlignment="Top" HorizontalAlignment="Right" TextAlignment="Right" Margin="0, 10, 10, 0" Foreground="White" FontSize="20">Alarm ON</TextBlock>
    ```

2. In `MainPage.xaml.cs`, add the following global variable.
    
    ```csharp
    private DispatcherTimer clockTimer;
    ```

3. Add the following code at the end of the OnNavigatedTo method.

    ```csharp
    // Choose Happiness as expected emotion
    expectedEmotion = labels[1];
    EmotionText.Text = $"Show {expectedEmotion} to Dismiss";
    
    clockTimer = new DispatcherTimer();
    clockTimer.Interval = TimeSpan.FromMilliseconds(300);
    clockTimer.Tick += Timer_Tick;
    clockTimer.Start();
    ```

4. Add the code for the Timer_Tick handler.

    ```csharp
    private void Timer_Tick(object sender, object e)
    {
        TimeText.Text = DateTime.Now.ToString("HH:mm:ss");
    }
    ```

## 4. Add the alarm

For simplicity, the alarm will start when the application starts and turn off when the desired emotion is detected for at least 3 seconds.

1. Add the following global variables:

    ```csharp
    private bool alarmOn = true;
    private SolidColorBrush red = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
    private SolidColorBrush white = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
    ```

2. At the beginning of the AnalyzeFrame method, add the code below to avoid analyzing images if the alarm hasn't triggered.

    ```csharp
    if (!alarmOn)
        return;
    ```

3. At the end of the Timer_Tick method, add the code below. This will make the background blink from red to black while the alarmOn variable is true.

    ```csharp
    if (alarmOn)
    {
        TimeText.Foreground = TimeText.Foreground == white ? red : white;
        AlarmText.Text = "Alarm ON";
    }
    else
    {
        TimeText.Foreground = white;
        AlarmText.Text = "Alarm OFF";
    }
    ```

## 5. Stop the alarm when emotion is detected

Finally, we need to stop the alarm when the required emotion (happiness) is detected.

1. Add the following method to check if the alarm must be turned off:

    ```csharp
        private async Task ProcessEmotion(string detectedEmotion)
        {
            if (!string.IsNullOrWhiteSpace(detectedEmotion) && (expectedEmotion.Equals(detectedEmotion, StringComparison.CurrentCultureIgnoreCase)))
                {
                alarmOn = false;
            }
        }
    ```

2. Add a call to ProcessEmotion at the end of the try block in the AnalyzeFrame method:

    ```csharp
    try
    {
        detectedEmotion = await DetectEmotionWithWinML();
        await ProcessEmotion(detectedEmotion);
    }
    ```

That's it! Run the application again, and the alarm will be dismissed when happiness is detected :)
