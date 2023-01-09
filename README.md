# Tracking-vessel-motion-in-ultrasound-images-using-yolov5
Liver Ultrasound Vascular Tracking Task in Accurate Radiotherapy Guided by Ultrasound  
- Python trains the YoloV5-Tiny detection model and converts it into a Libtorch model
- C++ postprocesses it and generates a DLL file
- Based on C# winform, an interface for visualizing vascular motion curves is developed

# Effect
Real-time detection and frame rate of around 70FPS can be achieved
![1673247730556-92538312-0a5d-47eb-a8eb-d3269b1ec13b](https://user-images.githubusercontent.com/56248224/211257245-843bb765-0c4c-4b12-9c00-93631360ef63.png)

# Enviroment

**Python:**
- Python 3.7
- torch 1.7.1

**C++:**
- VS：2019
- opencv：4.5.4 vc15
- libtorch：libtorch1.7.1 for cuda110
- cuda : cudnn cuda11.0

**C#:**
- .Net Framework 4.7.2
- OpencvSharp4

