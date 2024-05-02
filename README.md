# SilkRenderer
## WPF、WinUI3 使用 Silk.NET 绘制示例（OpenGL、DirectX）

## WPF、WinUI3 Use Silk.NET to draw examples (OpenGL, DirectX)

### 注意事项
该项目代码结构较为繁琐复杂，代码结构比较混乱，仅供参考学习。

### Precautions
The project code structure is more cumbersome and complex, and the code structure is relatively messy, only for reference and learning.

### 项目说明
- 本项目是一个使用 Silk.NET 绘制图形的示例项目，包含了 WPF 和 WinUI3 两个项目，分别使用 OpenGL 和 DirectX 两种渲染方式。			
- 渲染模式依靠 OpenTK 提供的渲染思路，使用 NV_DX_Interop 扩展实现 DirectX 与 OpenGL 之间的交互。
- 使用该方式进行渲染，能够极大提高 WPF 和 WinUI3 的图形性能，同时也能够接入不同的图形库。

### Project Description
- This project is an example project that uses Silk.NET to draw graphics. It contains two projects, WPF and WinUI3, which use OpenGL and DirectX rendering respectively.
- The rendering mode relies on the rendering ideas provided by OpenTK and uses the NV_DX_Interop extension to implement interaction between DirectX and OpenGL.
- Using this rendering method can greatly improve the graphics performance of WPF and WinUI3, and can also access different graphics libraries.

### 项目结构
- RenderContext：渲染上下文，用户管理 OpenGL 和 DirectX 的渲染环境。
- Framebuffer：帧缓冲，用户管理 OpenGL 和 DirectX 的帧缓冲。
- GameControl：渲染控件，管理上下文和渲染。

### Project Structure
- RenderContext: Rendering context, user management OpenGL and DirectX rendering environment.
- Framebuffer: Frame buffer, user management OpenGL and DirectX frame buffer.
- GameControl: Rendering control, management context and rendering.
