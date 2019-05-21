# UWP Demo for Insider Dev Tour 2019

This repo contains a demo UWP app that is built for demostrating rich controls and libraries from Windows UI Library and Windows Community Toolkit.

## Getting Started

These instructions will get you a copy of the project up and running on your local Windows 10 machine.

### Prerequisites

1. Windows 10 1903
2. [Visual Studio 2019](https://visualstudio.microsoft.com/downloads/)
3. [Windows 10 SDK 18362](https://blogs.windows.com/buildingapps/2019/04/02/windows-10-sdk-preview-build-18362-available-now/#5gh4edOeTx1T1Yj2.97)

## Demo steps

You should see a more compelling demo after finishing all the steps below. 

### **TIP**: Use Task List to guide you through the steps

Do a search on *"TODO"* in Visual Studio and use <kbd>Ctrl</kbd> + <kbd>K</kbd> + <kbd>H</kbd> to bookmark all the TODOs from XAML since TODOs in XAML are not automatically inserted into the Task List. In the end, you should see a list like the one below:

Clicking on each item from the Task List will take you directly to a predefined location in the code.

#### Step 1.1 `ItemsRepeater`

*Click on the first *TODO* item to navigate to the code, do the same for the rest of the steps.*

Remove the `ListView` from XAML and uncomment out the `ScrollViewer` below that contains the `ItemsRepeater`.

#### Step 1.2 `ItemsRepeater`

Uncomment out the `Loaded` event.

After you have done [1.1](#step-11-itemsrepeater) & [1.2](#step-12-itemsrepeater), you should see a carousel-like `ItemsRepeater` control implemented on the UI.

#### Step 2.0 `AcrylicBrush`

Uncomment out the `Rectangle` that is colored with the `AcrylicBrush`.

After you have done [2.0](#step-20-acrylicbrush), you should see a blurred background that gets updated to a new image every time you select a different one from the `ItemsRepeater` control.

#### Step 3.1 `TeachingTip`

Check out how the three `TeachingTip`s are defined inside a `UserControl`.

#### Step 3.2 `TeachingTip`

Check out how the `Target` of the `TeachingTip` is setup.

After you have gone through [3.1](#step-31-teachingtip) & [3.2](#step-32-teachingtip), click on the circular button from the bottom of the `NavigationView` to see how the *step-by-step guide* works.

#### Step 4.1 `ImplicitAnimations`

Add `animations:Implicit.Animations="{StaticResource ImplicitOffsetAnimation}"` to `controls:ImageView`, `controls:MissionView`, `controls:GaugeView` & `controls:JournalView`.

For example,
```
<controls:ImageView x:Name="ImageView"
                    animations:Implicit.Animations="{StaticResource ImplicitOffsetAnimation}"
                    ImageUpdated="ImageView_ImageUpdated">
```

After you have done [4.1](#step-41-implicit-animations), you should see components smoothly move to their new position when you resize the app window.

#### Step 4.2 `ImplicitAnimations`

Remove the default `Navigate` method with the new one that suppresses the default navigation animation.

#### Step 4.3 `ImplicitAnimations`

Uncomment out animation code on `controls:ImageView`, `controls:MissionView`, `controls:GaugeView` & `controls:JournalView`. 

For example,
```
<controls:ImageView x:Name="ImageView" ImageUpdated="ImageView_ImageUpdated">
    <animations:Implicit.ShowAnimations>
        <animations:TranslationAnimation Delay="0:0:0.2"
                                          SetInitialValueBeforeDelay="True"
                                          From="0,-80,0"
                                          To="0,0,0"
                                          Duration="0:0:1.2" />
        <animations:OpacityAnimation Delay="0:0:0.2"
                                      SetInitialValueBeforeDelay="True"
                                      From="0"
                                      To="1"
                                      Duration="0:0:1.2" />
    </animations:Implicit.ShowAnimations>
    <animations:Implicit.HideAnimations>
        <animations:TranslationAnimation From="0,0,0"
                                          To="0,-80,0"
                                          Duration="0:0:0.6" />
        <animations:OpacityAnimation From="1"
                                      To="0"
                                      Duration="0:0:0.6" />
    </animations:Implicit.HideAnimations>
</controls:ImageView>
```

After you have done [4.2](#step-42-implicit-animations) & [4.3](#step-43-implicit-animations), try to navigate from the current page to another page to see customized page animations.

#### Step 5.1 `SceneLoader`

Check out how to use the `SceneLoader` API *(Nuget link missing atm)* to load a `.gltf` file into a `SceneNode`.

#### Step 5.2 `SceneLoader`

Check out how to attach the `SceneNode` to UI.

After you have gone through [5.1](#step-51-sceneloader) & [5.2](#step-52-sceneloader), you should have a basic understanding of how the `SceneLoader` works.

#### Step 6.1 `AnimatedVisualLayer`

Uncomment out the `winui:AnimatedVisualPlayer` control from XAML.

#### Step 6.2 `AnimatedVisualLayer`

Uncomment out the code that shows the Lottie animation.

#### Step 6.3 `AnimatedVisualLayer`

Uncomment out the code that hides the Lottie animation.

After you have done [6.1](#step-61-animatedvisuallayer), [6.2](#step-62-animatedvisuallayer) & [6.3](#step-63-animatedvisuallayer), you should now see a paper plane animation while loading the 3D model.






