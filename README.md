## **About the XlGameTemplate Unity project**

### **A simple C# SDK integration sample**

XlGameTemplate is a simple C# SDK integration sample on Unity; Its goal is to deliver you a way to use most of the SDK features in your project with sober and generic displays. You may use it as is or copy/paste only some parts at your convenience and modify the graphics or the logic to feat your needs.

This template is thought to be quickly plugged into your game (only a few minutes for most basic features) and allow you to enhance the social aspect of your game in no efforts! Obviously, you can freely use it and customize it to match your desires.

For more details about the SDK features, please refer to our [online backend documentation](http://doc.xtralife.cloud/backend/).

### **How to?**

A very few simple steps are required to use this game template within your Unity project:

I. Import the template into your project:

- a) If you didn't included it already, get the **CotcSdk core files** by copying the **CotcSdk** and **Plugins** folders from **[GitRepositoryRoot]\UnityProject\Assets**.

- b) Get the **game template files** by copying the **CotcSdkTemplate** folder from **[GitRepositoryRoot]\UnityProject\Assets**.

 >(You may already open the **SampleScene** scene to discover all features integrated by the template just by following the step **II.b** beforehand)

II. Now you'll need to use your game's credentials (**API Key** and **Secret**) you can retrieve from your **FrontOffice** to allow the use of the SDK features:

- a) Add a **CotcSdk** prefab's instance in your scene.

- b) On the **CotcGameObject** script of the **CotcSdk** object, fill in your game **API Key** and **Secret**.

 ![Screenshot-CotcGameObject](https://github.com/xtralifecloud/unity-gametemplate/raw/master/readme/Screenshot-CotcGameObject.png)

 >(For your convenience, you may use the following credentials to hit the **XlGameTemplate sandbox** environment instead of your game's one; **API Key:** *jO6h9ndwmHRS0OX*, **API Secret:** *u5irMS8QqNp5YWzkDnML*)

III. The last step is simply to use the template prefabs and scripts you just included:

- a) For each *"module"* you intend to display, just include a **CotcPanel-*Handler** (e.g. **CotcPanel-LeaderboardHandler** to display the **leaderboard** scores) prefab's instance in your scene as a child of a **Canvas** object to allow the generic display of the corresponding features.
>**Hint 1:** Please ensure the **CotcPanel-*Handlers** are the last children (top down of the list) in their level of your UI objects hierarchy, so they will come on the front of the screen and won't be hidden by other objects when displayed (as they act like popups).
>
>![Screenshot-ObjectsHierarchy](https://github.com/xtralifecloud/unity-gametemplate/raw/master/readme/Screenshot-ObjectsHierarchy.png)
>
>**Hint 2:** For a better display result, we recommend the **Canvas** to have the **Render Mode** set to **Screen Space - Overlay** and the **Canvas Scaler** to have the **UI Scale Mode** set to **Scale With Screen Size**, so the **CotcPanels** will always come on the top of your camera rendering and you can handle portrait and landscape orientations with more ease.
>
>![Screenshot-CanvasSettings](https://github.com/xtralifecloud/unity-gametemplate/raw/master/readme/Screenshot-CanvasSettings.png)
>
>**Hint 3:** Still for a better display result, we recommend to **stretch** the **CotcPanels** (horizontaly and verticaly, full screen if you can, clicking with the **Alt + Shift** keys down to set anchor position and pivot at the same time) once you included them in your **Canvas** to ensure they are positioned in the correct bounds and have all the available space at their disposal.
>
>![Screenshot-PanelStretch](https://github.com/xtralifecloud/unity-gametemplate/raw/master/readme/Screenshot-PanelStretch.png)
>
>**Hint 4:** If you are using complex UI structures, you are probably working with multiple **Layout Group** components throughout your **Canvas** children objects. While you are positioning/stretching the **CotcPanels**, please keep in mind that the **Layout Group** components may influence the panels positioning and sizing even if they have no **Layout Element** component attached.

- b) Attach the **SampleScript** component to any object in your scene.
>(You may want to create **IntputFields** and link them into the corresponding serialized fields of this script to allow those inputs to replace the default values)

- c) For each feature you intend to try, just create a button and link it to the appropriate method on the **SampleScript** (e.g. **Button_DisplayAllHighScores()**).
>(You're not forced to use buttons to trigger SDK features however. Please notice that **SampleScript** make some initialization calls at **Start** to ensure the SDK is initialized, that you get automatically logged in as a gamer, and that you listen for incoming events)

- d) Run the scene, wait to be logged in, then enjoy! =)

### **To get a bit further...**

This game template is only one of the infinite ways to integrate the XtraLife C# SDK. Do not hesitate to twist and mold it at your convenience! If you are willing to do that, you may want to have a look to the **SampleScript** as everything starts from here and it represents your *"game-side"* calls.

The template's scripts structure mainly splits in 2 parts:

I. The -Features scripts are basically holding the methods you will call from your scripts and buttons and they form the main logic (in two words: **"get/send data"**). They contain:

- In the **Handling** code region: any internal logic to execute prior to the SDK methods calls.

- In the **Backend** code region: only simple calls to the SDK methods and execution of success and error callbacks.

- In the **Delegate Callbacks** code region: the logic to react to success and error results.

- In the **Events Callbacks** code region: the logic to trigger and react to some events (e.g. **OnGamerLoggedIn**).

II. The -Handler scripts are UI handlers with the task to show results into a user convenient format (in two words: **"display results"**). They contain:

- In the **Display** code region: all what's necessary to display the results.

- In the **Buttons Actions** code region: actions to be triggered by buttons (e.g. **Previous/Next** pages in leaderboard).
