## **About the XlGameTemplate Unity project**

### **A standard C# SDK integration sample**

XlGameTemplate is a simple C# SDK integration sample on Unity; Its goal is to deliver you a way to use most of the SDK features in your project with sober and generic displays. You may use it as is or copy/paste only some parts at your convenience and modify the graphics or the logic to feat your needs.

This template is made to be quickly plugged into your game (only a few minutes for most basic features) and allow you to enhance the social aspect of your game in no efforts! Obviously, you can freely use it and customize it to match your desires.

### **How to?**

A few very simple steps are required to use the game template within your Unity project:

A. Get and open **XlGameTemplate.unitypackage** from the root of the Git repository to include the template and the C# SDK core files in your game.

 >*(You may already open the **SampleScene** scene to discover all features integrated by the template just by following the step **2.b**)*

B. Now you'll need to use the game credentials (**API Key** and **Secret**) you can retrieve from your **FrontOffice** to allow the use of the SDK:

- 1) Include a **CotcSdk** prefab's instance in your scene.

- 2) On the **CotcGameObject** script of the **CotcSdk** object, fill in your game **API Key** and **Secret**.

 ![Screenshot-CotcGameObject](http://gitlab.xtralife.cloud/xtralife/xtralife-unity-gametemplate/raw/master/readme/Screenshot-CotcGameObject.png)

C. The last step is simply to use the template prefabs and scripts you previously included from the package:

- 1) For each module you intend to display, just include a **CotcPanel-*Handler** (e.g. **CotcPanel-LeaderboardHandler** for the **leaderboard** scores) prefab's instance in your scene with a **Canvas** object as parent to allow the generic display of the corresponding features.
>**Hint 1:** Please ensure the **CotcPanels** are the last children (top down of the list) in their level of your UI objects hierarchy, so they will come in the front of the screen when displayed (as they act like popups).
>
>![Screenshot-ObjectsHierarchy](http://gitlab.xtralife.cloud/xtralife/xtralife-unity-gametemplate/raw/master/readme/Screenshot-ObjectsHierarchy.png)
>
>**Hint 2:** For a better display result, we recommend the canvas to have the **Render Mode** set to **Screen Space - Overlay** and the **Canvas Scaler** to have the **UI Scale Mode** set to **Scale With Screen Size**, so the **CotcPanels** will always come on the top of your UI and you can handle portrait and landscape orientations with more ease.
>
>![Screenshot-CanvasSettings](http://gitlab.xtralife.cloud/xtralife/xtralife-unity-gametemplate/raw/master/readme/Screenshot-CanvasSettings.png)
>
>**Hint 3:** Still for a better display result, we recommend to **stretch** the **CotcPanels** (horizontaly and verticaly, full screen if you can) once you included them in your **Canvas** to ensure they are positioned in the correct bounds and have all the available space at their disposal.
>
>![Screenshot-PanelStretch](http://gitlab.xtralife.cloud/xtralife/xtralife-unity-gametemplate/raw/master/readme/Screenshot-PanelStretch.png)
>
>**Hint 4:** If you are using complex UI structures, you probably have multiple **Layout Group** components in your **Canvas** children objects. When you are positioning/stretching the **CotcPanels**, please keep in mind that the **Layout Group** components may influence the panels positioning even if they have no **Layout Element** component attached.

- 2) Attach the **SampleScript** to any object in your scene.
>*(You may want to create **IntputFields** and link them into the corresponding serialized fields of the script to allow those inputs to replace the default script values)*

- 3) For each feature you intend to use, just create a button and link it to the appropriate method on the **SampleScript** (e.g. **Button_DisplayAllHighScores()**).
>*(You don't have to use buttons to trigger SDK features. Please notice that **SampleScript** calls the **Cloud** initilization and an **AutoLogin** method at **Start** to ensure the SDK is initialized and you get automatically logged in as a gamer)*

- 4) Run the scene, wait to be logged in, then enjoy! =)
>*(Please note that obviously you will need to post scores or define achievements before you can display them on the **CotcPanels**)*

### **To get a bit further...**

This game template is only one of the infinite ways to integrate the XtraLife C# SDK. Do not hesitate to twist and mold it at your convenience! If you are willing to do that, you may want to have a look to the **SampleScript** as everything starts from here.

The template scripts structure mainly splits in 2 parts:

1. The ***Features** scripts are basically holding the methods you will call from your scripts and buttons and they form the main logic (in a word: **"get/send data"**). They contain:

- a) In the **Handling** code region: any internal logic to execute prior to the SDK methods.

- b) In the **Features** code region: only simple calls to the SDK methods and execution of success and error callbacks.

- c) In the **Delegate Callbacks** code region: the logic to react to success and error results.

2. The ***Handler** scripts are UI handlers with the task to show results into a user convenient format (in a word: **"display results"**). They contain:

- a) In the **Display** code region: all what's necessary to display the results.

- b) In the **Buttons Actions** code region: actions to be triggered by buttons (e.g. previous/next page in leaderboard).
