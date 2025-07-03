# 🎨 Image Color Quantizer

A fast and simple command-line tool written in C# to quantize an image into a reduced color palette using K-Means clustering.

![Cow Banner](./ReadMeImages/CowBanner.png)  
<sub>📷 Photo by [Ave Calvar Martinez](https://www.pexels.com/photo/selective-focus-photo-of-a-brown-cow-3656870/)</sub>

---

## ✨ Features

- ⚡ Fast and efficient color quantization
- 🎯 Reduces colors using **K-Means clustering**
- 🧼 Automatically ignores **fully transparent pixels**
- 🖼️ Supports common image formats: **PNG**, **JPG**, etc.
- 🎨 Outputs a clean version with a defined number of dominant colors
- 🧰 Easy to run, customize, and extend from the command line

---

## 🚀 Usage

```bash
dotnet run -- Images/<FileName> (optional: <NumberOfColors>)
```
---
## 📦 Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/mdomnik/ImageColorQuantizer.git
   ```
2. Navigate to the project directory:
   ```bash
   cd ImageColorQuantizer
   ```
3. Restore dependencies:
   ```bash
   dotnet restore .\Image_ColorQuantizer.sln
   ```
4. Run the tool:
   ```bash
   dotnet run -- Images/input.png 4
   ```
---
## 🖼️ Showcase
Below is an example of the tool applied to a colorful image, showing different quantization levels side by side:

![Bird Banner](./ReadMeImages/BirdShowcase.png)
<sub>📷 Photo by Alex P: https://www.pexels.com/photo/blue-and-yellow-macaw-perched-on-twig-2078772/</sub>