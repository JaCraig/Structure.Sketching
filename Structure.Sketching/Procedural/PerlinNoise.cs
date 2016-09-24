/*
Copyright 2016 James Craig

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using Structure.Sketching.ExtensionMethods;

namespace Structure.Sketching.Procedural
{
    /// <summary>
    /// Perlin noise helper class
    /// </summary>
    public static class PerlinNoise
    {
        /// <summary>
        /// Generates perlin noise
        /// </summary>
        /// <param name="Width">Width of the resulting image</param>
        /// <param name="Height">Height of the resulting image</param>
        /// <param name="MaxRGBValue">MaxRGBValue</param>
        /// <param name="MinRGBValue">MinRGBValue</param>
        /// <param name="Frequency">Frequency</param>
        /// <param name="Amplitude">Amplitude</param>
        /// <param name="Persistance">Persistance</param>
        /// <param name="Octaves">Octaves</param>
        /// <param name="Seed">Random seed</param>
        /// <returns>An image containing perlin noise</returns>
        public static Image Generate(int Width, int Height, float MaxRGBValue, float MinRGBValue,
            float Frequency, float Amplitude, float Persistance, int Octaves, int Seed)
        {
            var ReturnValue = new Image(Width, Height, new byte[Width * Height * 4]);
            var Noise = GenerateNoise(Seed, Width, Height);
            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    var Value = GetValue(x, y, Width, Height, Frequency, Amplitude, Persistance, Octaves, Noise);
                    Value = (Value * 0.5f) + 0.5f;
                    Value *= 255;
                    var RGBValue = (byte)Value.Clamp(MinRGBValue, MaxRGBValue);
                    ReturnValue.Pixels[((y * Width) + x) * 4] = RGBValue;
                    ReturnValue.Pixels[(((y * Width) + x) * 4) + 1] = RGBValue;
                    ReturnValue.Pixels[(((y * Width) + x) * 4) + 2] = RGBValue;
                    ReturnValue.Pixels[(((y * Width) + x) * 4) + 3] = 255;
                }
            }
            return ReturnValue;
        }

        private static float[,] GenerateNoise(int Seed, int Width, int Height)
        {
            float[,] Noise = new float[Width, Height];
            var RandomGenerator = new System.Random(Seed);
            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    Noise[x, y] = ((float)RandomGenerator.NextDouble() - 0.5f) * 2.0f;
                }
            }
            return Noise;
        }

        private static float GetSmoothNoise(float X, float Y, int Width, int Height, float[,] Noise)
        {
            if (Noise == null)
                return 0.0f;
            float FractionX = X - (int)X;
            float FractionY = Y - (int)Y;
            int X1 = ((int)X + Width) % Width;
            int Y1 = ((int)Y + Height) % Height;
            int X2 = ((int)X + Width - 1) % Width;
            int Y2 = ((int)Y + Height - 1) % Height;

            float FinalValue = 0.0f;
            FinalValue += FractionX * FractionY * Noise[X1, Y1];
            FinalValue += FractionX * (1 - FractionY) * Noise[X1, Y2];
            FinalValue += (1 - FractionX) * FractionY * Noise[X2, Y1];
            FinalValue += (1 - FractionX) * (1 - FractionY) * Noise[X2, Y2];

            return FinalValue;
        }

        private static float GetValue(int X, int Y, int Width, int Height, float Frequency, float Amplitude,
            float Persistance, int Octaves, float[,] Noise)
        {
            if (Noise == null)
                return 0.0f;
            float FinalValue = 0.0f;
            for (int i = 0; i < Octaves; ++i)
            {
                FinalValue += GetSmoothNoise(X * Frequency, Y * Frequency, Width, Height, Noise) * Amplitude;
                Frequency *= 2.0f;
                Amplitude *= Persistance;
            }
            FinalValue = FinalValue.Clamp(-1.0f, 1.0f);
            return FinalValue;
        }
    }
}