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

using Structure.Sketching.Formats.BaseClasses;
using Structure.Sketching.Formats.Png.Format.Helpers;
using System;
using System.Collections.Generic;
using System.IO;

namespace Structure.Sketching.Formats.Png.Format
{
    /// <summary>
    /// PNG File
    /// </summary>
    /// <seealso cref="Structure.Sketching.Formats.BaseClasses.FileBase" />
    public class File : FileBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="File" /> class.
        /// </summary>
        public File()
        {
            ChunkActions = new Dictionary<ChunkTypes, Action<Chunk>>
            {
                [ChunkTypes.Header] = ReadHeader,
                [ChunkTypes.Physical] = x => { },
                [ChunkTypes.Palette] = ReadPalette,
                [ChunkTypes.TransparencyInfo] = ReadAlphaPalette,
                [ChunkTypes.Text] = ReadText,
                [ChunkTypes.End] = x => { },
                [ChunkTypes.Data] = ReadData
            };
        }

        /// <summary>
        /// Gets the alpha palette.
        /// </summary>
        /// <value>
        /// The alpha palette.
        /// </value>
        public Palette AlphaPalette { get; private set; }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public Data Data { get; private set; }

        /// <summary>
        /// Gets the file end.
        /// </summary>
        /// <value>
        /// The file end.
        /// </value>
        public FileEnd FileEnd { get; private set; }

        /// <summary>
        /// Gets or sets the file header.
        /// </summary>
        /// <value>
        /// The file header.
        /// </value>
        public FileHeader FileHeader { get; private set; }

        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <value>
        /// The header.
        /// </value>
        public Header Header { get; private set; }

        /// <summary>
        /// Gets the palette.
        /// </summary>
        /// <value>
        /// The palette.
        /// </value>
        public Palette Palette { get; private set; }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public List<Property> Properties { get; private set; }

        /// <summary>
        /// Gets or sets the chunk actions.
        /// </summary>
        /// <value>
        /// The chunk actions.
        /// </value>
        private IDictionary<ChunkTypes, Action<Chunk>> ChunkActions { get; set; }

        /// <summary>
        /// Decodes the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>
        /// This.
        /// </returns>
        public override FileBase Decode(Stream stream)
        {
            FileHeader = FileHeader.Read(stream);
            var Chunks = ReadChunks(stream);
            ParseChunks(Chunks);
            return this;
        }

        /// <summary>
        /// Writes to the specified stream.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="animation">The animation.</param>
        /// <returns>
        /// True if it writes successfully, false otherwise.
        /// </returns>
        public override bool Write(BinaryWriter writer, Animation animation)
        {
            return Write(writer, animation[0]);
        }

        /// <summary>
        /// Writes to the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="image">The image.</param>
        /// <returns>
        /// True if it writes successfully, false otherwise.
        /// </returns>
        public override bool Write(BinaryWriter stream, Image image)
        {
            LoadImage(image);
            FileHeader.Write(stream);
            ((Chunk)Header).Write(stream);
            ((Chunk)Data).Write(stream);
            ((Chunk)FileEnd).Write(stream);
            return true;
        }

        /// <summary>
        /// Converts the file to an animation.
        /// </summary>
        /// <returns>
        /// The animation version of the file.
        /// </returns>
        protected override Animation ToAnimation()
        {
            return new Animation(new Image[] { ToImage() }, 0);
        }

        /// <summary>
        /// Converts the file to an image.
        /// </summary>
        /// <returns>
        /// The image version of the file.
        /// </returns>
        protected override Image ToImage()
        {
            return Data.Parse(Header, Palette, AlphaPalette);
        }

        /// <summary>
        /// Loads the image.
        /// </summary>
        /// <param name="image">The image.</param>
        private void LoadImage(Image image)
        {
            FileHeader = new FileHeader();
            Header = new Header(image);
            Data = new Data(image);
            FileEnd = new FileEnd();
        }

        /// <summary>
        /// Parses the chunks.
        /// </summary>
        /// <param name="chunks">The chunks.</param>
        private void ParseChunks(IEnumerable<Chunk> chunks)
        {
            foreach (var CurrentChunk in chunks)
            {
                if (ChunkActions.ContainsKey(CurrentChunk.Type))
                    ChunkActions[CurrentChunk.Type](CurrentChunk);
            }
        }

        /// <summary>
        /// Reads the alpha palette.
        /// </summary>
        /// <param name="obj">The object.</param>
        private void ReadAlphaPalette(Chunk obj)
        {
            AlphaPalette = obj;
        }

        /// <summary>
        /// Reads the chunks.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>A list of chunks from the stream</returns>
        private List<Chunk> ReadChunks(Stream stream)
        {
            var Results = new List<Chunk>();
            while (true)
            {
                var TempChunk = Chunk.Read(stream);
                if (TempChunk == null)
                    break;
                Results.Add(TempChunk);
            }
            return Results;
        }

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="obj">The object.</param>
        private void ReadData(Chunk obj)
        {
            if (Data != null)
                Data = Data + (Data)obj;
            else
                Data = obj;
        }

        /// <summary>
        /// Reads the header.
        /// </summary>
        /// <param name="obj">The object.</param>
        private void ReadHeader(Chunk obj)
        {
            Header = obj;
        }

        /// <summary>
        /// Reads the palette.
        /// </summary>
        /// <param name="obj">The object.</param>
        private void ReadPalette(Chunk obj)
        {
            Palette = obj;
        }

        /// <summary>
        /// Reads the text.
        /// </summary>
        /// <param name="obj">The object.</param>
        private void ReadText(Chunk obj)
        {
            Properties.Add(obj);
        }
    }
}