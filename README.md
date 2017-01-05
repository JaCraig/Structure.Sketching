# Structure.Sketching

[![Windows Build Status](https://ci.appveyor.com/api/projects/status/2v806lb18724mr1t?svg=true)](https://ci.appveyor.com/project/JaCraig/structure-sketching)

[![Linux Build Status](https://travis-ci.org/JaCraig/Structure.Sketching.svg?branch=master)](https://travis-ci.org/JaCraig/Structure.Sketching)

Structure.Sketching is an image processing library for use with .Net Core and .Net 4.6. Since System.Drawing is not really a thing in .Net Core there was a need for a very simple image processing library. This library is in response to that fact and supports .Net Core, .Net 4.6, Mono, and UWP. While currently in beta, the library is fairly usable at this stage. The namespaces may change for individual classes as it is cleaned up for production but otherwise it should be fairly safe to use.

## Supported Formats/Filters

Currently the library supports the following file types:

* JPG
* BMP - Reading: 32bit, 24bit, 16bit, 8bit, 8bit RLE, 4bit, and 1bit. Writing: 24bit
* PNG - Reading: RGB, RGBA, Greyscale, Greysale+alpha, Palette. Writing: RGBA
* GIF - Includes animation support

There are also a number of filters within the library for a number of different purposes:

- Contrast stretching
- Gamma correction
- HSV and RGB equalization
- Adaptive HSV and RGB equalization
- Kuwahara smoothing
- Median smoothing
- SNN Blur
- Image resizing
- Image cropping
- Canvas resizing
- Image flipping
- Affine transformations including:
  - Rotatation
  - Scaling
  - Skewing
  - Translatation
- Image resizing and affine transformations can use the following resampling filters:
  - Bell
  - Bicubic
  - Bilinear
  - Catmull Rom
  - Cosine
  - Cubic B Spline
  - Cubic Convolution
  - Hermite
  - Lanczos3
  - Lanczos8
  - Mitchell
  - Nearest Neighbor
  - Quadratic B Spline
  - Quadratic
  - Robidoux
  - Robidoux Sharp
  - Robidoux Soft
  - Triangle
- Bump map generation
- Canny Edge Detection
- Gaussian Blur
- Lomograph
- Normal map generation
- Polaroid
- Image blending
- Glow
- Vignette
- Constrict
- Dilate
- Unsharp
- Box Blur
- Embossing
- Edge detection techniques including:
  - Kayyali
  - Kirsh
  - Laplace Edge Detection
  - Laplacian of Gaussian Edge Detection
  - Prewitt
  - Roberts Cross
  - Robinson
  - Scharr
- Sharpen
- Sharpen Less
- Sobel Embossing
- Alpha manipulation
- Black and White
- Blue, green, and red filters
- Brigtness manipulation
- Contrast manipulation
- Greyscale 601 and 709
- Hue manipulation
- Kodachrome
- Saturation
- Sepia Tone
- Color blindness filters including: Achromatomaly, Achromatopsia, Deuteranomaly, Deuteranopia, Protanomaly, Protanopia, Tritanomaly, and Tritanopia
- Adaptive Threshold
- Non Maximal Suppression
- Threshold
- Image addition, subtraction, division, multiplication, modulo, and, or, and xor functions.
- Turbulence
- Solarize
- Sin Wave
- Posterize
- Pointillism
- Pixellate
- Noise
- Logarithm
- Jitter
- Color replacement
- Color inversion
- There are also generic classes for color matrix (using a 5x5 matrix), affine transformations, and convolution filters.

There are also a couple of other items in here including:

- Perlin Noise generation
- The library also includes the ability to draw lines, rectangles, and ellipses, both filled and the outline.
- Image to ASCII art
- Image to Base64 string

That said hopefully the list will grow with time.

## Usage

Generally speaking the library is fairly simple to use:

    new Image("ExampleImage.jpg")
        .Apply(new CannyEdgeDetection(Color.Black, Color.White, .9f, .1f))
        .Save("ExampleImage2.jpg");
		
The Image class itself has a Fluent interface and can accept a string pointing to the file to load, a stream, or a byte array along with a width and height if you want a blank image. The Apply function applies a filter to the image, however you can specify a rectangle as the second parameter if you want it on only a portion of the image. The save function takes either a file name or a stream and an enum specifying the format to save it as.

For further explanation of the filters, please see the wiki documentation... Once I've written it.

## Installation

The library is available via Nuget with the package name "structure.sketching". To install it run the following command in the Package Manager Console:

Install-Package structure.sketching

## Build Process

In order to build the library you will require the following:

1. Visual Studio 2015 with Update 3
2. .Net Core 1.0 SDK

Other than that, just clone the project and you should be able to load the solution and build without too much effort.
