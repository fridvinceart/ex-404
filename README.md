powered by **fridvince**

THANKS:
chatGPT for assistance and code review!

PREVIEW:
https://youtu.be/CMaoQD43wjQ?si=2uYobh6clbQjjhbK

LICENSE:
fridvince@gmx.us
----------------------------------------------------------------------------------

Requirements

    **Unity version: 2023.2.20f1**
    **Environment: 3D (Build-in Render Pipeline)**

Start

    **Cinemachine**
    **Text Mesh Pro (Essentials and Examples)**
    **Timeline 1.8.7**

                            **@@@ DISTORTION CONTROLLERS @@@**

UIDistortionController.cs
{
    This script provides an easy way to animate and distort UI elements like images by applying offset animations to the four corners. It uses AnimationCurve to control the distortion over time, with adjustable speed and looping options. It is ideal for creating dynamic UI transitions or special effects.
    
    Features:
    {
        Independent Corner Control customizeable the distortion for each corner with separate AnimationCurve for X and Y axes
        Time-Based Animation controls the speed and loop behavior of the distortion effect
        Use transparency cutoff to mask parts of the image as it distorts
        9-Slice Border Support is a key feature to define borders for UI elements to ensure the distortion effect works seamlessly on sliced images.
    }
    
    
    **How to Use:
    Attach the UIDistortionController script to any Image component
    Set the animation curves for each corners distortion
    Customize the animation behavior (speed, looping, etc.).
    The script will automatically apply the distortion and update the shader in real-time.**
}
----------------------------------------------------------------------------------

UIDistortionMaskController.cs
{
    This script allows for custom distortion effects to UI elements like Image. By using a material with a custom shader (fridvince/UIDistortedMask), it applies distortion based on four corner offsets.
    
    Features:
    {
        Control the distortion amount for each of the four corners of the UI element independently.
        The material properties update automatically when the UI elements dimensions change, making it responsive to resizing or scaling.
        The materials properties are updated in real-time during runtime to reflect changes in the offsets.
    }
    
    **How to Use:
    Attach the UIDistortionMaskController script to any UI Image component.
    Assign a custom material or use the default fridvince/UIDistortedMask shader.
    Set the offsets for each corner (TopLeftOffset, TopRightOffset, BottomLeftOffset, BottomRightOffset).
    The distortion effect will automatically update in real-time as the image changes or is resized.**

    **Important Note for Masking:
    To achieve stable work with DistortionMask, please ensure that children objects of the parent with the UIDistortionMask.shader are using a shader specifically designed and respecting masking UIDistortionMaskChild.shader.
    UPD: Also Overlay Screen Space**
}
----------------------------------------------------------------------------------

                            **@@@ DISTORTION SHADERS @@@**

UIDistortion.shader
{
    Description:
    The UISlicedDistortion shader enables advanced UI rendering with dynamic corner-based distortion and customizable slicing borders.
    It is ideal for creating unique UI effects, offering precise control over corner offsets, borders, and alpha transparency.
    
    Features:
    Dynamic Corner Distortion:
    {
        Adjust offsets for each corner independently using _TopLeftOffset, _TopRightOffset, _BottomLeftOffset, and _BottomRightOffset properties.
        Slicing Borders: Control the slicing of the UI element via normalized UV coordinates with _BorderLeft, _BorderBottom, _BorderRight, and _BorderTop.
        Alpha Cutoff: Optional alpha threshold functionality to filter out pixels with low transparency (_AlphaCutoff).
        UI Optimization: Designed for the Unity UI system, ensuring compatibility with transparency and overlay rendering.
    }

    Technical Highlights:
    {
        Supports both vertex and fragment operations for seamless UI distortion.
        Utilizes the UnityObjectToClipPos function for correct transformations within the UI rendering pipeline.
        Includes fallback mechanisms for standard diffuse shading.
    }

    Visually dynamic UI elements, such as buttons, panels, or decorative overlays.
}
----------------------------------------------------------------------------------

UIDistortionMask.shader
{
    Description:
    The UIDistortedMask shader introduces a versatile masking solution for UI elements with dynamic corner-based distortion. It leverages stencil buffers to define precise mask areas, making it ideal for advanced UI designs that require dynamic visual effects.
    
    Features:
    {
        Corner-Based Distortion: Adjust offsets for the top-left, top-right, bottom-left, and bottom-right corners independently (_TopLeftOffset, _TopRightOffset, _BottomLeftOffset, _BottomRightOffset) for customized warping effects.
        Stencil Buffer Support: Employs a stencil reference (_StencilRef) to define and control the masked area for additional UI layering and masking.
        Smooth Offset Blending: Utilizes linear interpolation (lerp) to create seamless transitions between distorted corners.
    }

    Technical Highlights:
    {
        Compatible with the Unity UI system, rendering within the transparency queue for optimal blending.
        Implements vertex manipulation in the vert shader to handle corner offsets and masking behavior.
        Supports color multiplication, enabling dynamic adjustments or animations in the UI (v.color).
    }

    UI masks with flexible static distortion that respond dynamically to user interaction.
}
----------------------------------------------------------------------------------

UIDistortionMaskChild.shader
{
    Description:
    This variation of the UISlicedDistortion shader is designed specifically for masking child UI elements, offering flexible control over corner distortions and alpha-based visibility. It extends its functionality to enable seamless integration with UI layouts that require hierarchical masking.
    
    Features:
    {
       Child-Specific Distortion: Applies corner-based distortion (_TopLeftOffset, _TopRightOffset, _BottomLeftOffset, _BottomRightOffset) to child UI elements while respecting the parents boundaries.
       Configurable Borders: Allows precise control of masking edges through normalized UV coordinates (_BorderLeft, _BorderBottom, _BorderRight, _BorderTop), ensuring children align perfectly within parent regions.
       Alpha Cutoff Control: Filters out transparent areas based on the alpha threshold (_AlphaCutoff) to maintain a clean and polished UI look. 
    }
    
    Technical Highlights:
    {
        Uses vertex and fragment shaders for precise manipulation of child element positions and masking boundaries.
        Supports alpha-based pixel discarding, optimizing render performance and clarity.
        Integrates seamlessly with the Unity UI system, preserving hierarchy-based interactions.
    }
    
    Parent-child relationships in UI elements, such as clipped or masked child regions.
}
----------------------------------------------------------------------------------

                                **@@@ SHADER GRAPH @@@**

PrismaticSurfaceShader.shadergraph
NB! Universal Render Pipeline
----------------------------------------------------------------------------------

                        **@@@ AUXILIARY SCRIPTS (OPTIONAL) @@@**

UICommonButton.cs
{
    Description:
    The script provides a simple solution for buttons with clear visual feedback based on scale changes.
    It adjusts the buttons size to indicate interaction states like hover, click, and disabled.
    
    Features:
    {
        Scale-Based Feedback: Changes the buttons scale to visually reflect its states.
        Click Event Support: Executes a customizable onClick event when the button is clicked.
        Easy Setup: Plug-and-play script that works with any UI element.
    }
    
    Straightforward buttons with minimal setup and intuitive visual feedback.
}
---------------------------------------------------------------------------------

UISwipingGroup.cs
{
    Description:
    Provides a highly customizable solution for swipeable UI elements. It supports dynamic handling of selected states, allowing to attach multiple triggers and events for enhanced interactivity.
    
    Features:
    {
        Swipe Interaction enables horizontal swiping through items, automatically snapping to the nearest element based on swipe distance.
        Dynamic Selection State
        {
            Detects the currently selected item and dynamically triggers onSelected and onDeselected events.
            Supports attaching multiple events or animations to these states, providing flexibility in customization.
        }

    }
    
    Customizable Behavior:
    {
        Adjustable swipe sensitivity (swipeThreshold) and auto-snap distance (autoSwapDistance).
        Smooth position transitions with adjustable swipe speed.
        Event-Driven Design
        {
            Easily assign unique responses to each items selection or deselection.
            Perfect for creating interactive UI elements with dynamic reactions (e.g., animations, content updates, or visual effects).
        }
    }
    
    A powerful tool for creating interactive carousels, sliders, or swiping menus, where dynamic feedback and customization are key.
}
---------------------------------------------------------------------------------

CommonAutoRotation.cs
{
    The CommonAutoRotation script provides an easy way to apply continuous rotation to a target object in Unity, with customizable speed and optional looping. It's designed to offer simple, flexible control over automatic rotations without complex animation systems.
    
    Features:
    {
        Rotation Speed easily adjustable in 3D or UI space (X, Y, Z axes).
        Option to loop the rotation indefinitely or stop when a threshold is reached.
        Automatically resets the rotation back to its initial state when stopping (optional).
        Start and stop rotation via public methods, providing external control.
    }
    
    Rotating UI elements without the need for complex animation setups.
    
    **Usage:
    Attach the script to a GameObject.
    Assign the target Transform you want to rotate.
    Adjust the rotationSpeed for the desired effect.
    Use the StartRotation() and StopRotation() methods to control the rotation at runtime.**
    
    This script is lightweight and easy to integrate, providing seamless automatic rotation behavior with minimal setup!
}
----------------------------------------------------------------------------------

CommonInputRotation.cs
{
    Allows for smooth, interactive rotation of a target object based on user touch input. It enables intuitive object rotation by tracking touch movement on the screen and applying a rotation to the specified Transform. The script is useful for scenarios where you want to rotate objects based on user gestures, such as in touch-based interfaces or 3D object manipulation apps.
    
    Features:
    {
        Rotation for the target object based on touch input, allowing for intuitive control on mobile devices or touchscreens.
        Easily tweakable rotation speed to suit different use cases.
        Activate/Deactivate Rotation: Control the rotation behavior at runtime with the ActivateRotation() and DeactivateRotation() methods, allowing dynamic control over when rotation is enabled.
        Works with any Transform target and is simple to implement in your scene. Can be triggered by event.
    }
    
    **Usage:
    Attach the script to a GameObject.
    Assign the target Transform you want to rotate.
    Adjust the rotationSpeed to control how fast the rotation occurs.
    Use the ActivateRotation() and DeactivateRotation() methods to control when the rotation starts or stops.**

    This script provides a clean and efficient way to integrate touch-based rotation into Unity projects!
}
----------------------------------------------------------------------------------
----------------------------------------------------------------------------------
----------------------------------------------------------------------------------

                                **@@@ EXAMPLE 1 @@@**

So here is a preview how UIDistortion Shader works along with DistortionController curve animations. Buttons swap states by parent UISwapingGroup.cs
----------------------------------------------------------------------------------

                                **@@@ EXAMPLE 2 @@@**

And here is a setup with static distorted items which mask dinosaurs. Images animated by CommonAutoRotation.cs script and triggered by parent UISwapingGroup.cs
----------------------------------------------------------------------------------
                                
Enjoy!
