﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationExtensions.cs" company="nGratis">
//  The MIT License (MIT)
//
//  Copyright (c) 2014 - 2021 Cahya Ong
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// </copyright>
// <author>Cahya Ong - cahya.ong@gmail.com</author>
// <creation_timestamp>Friday, 28 April 2017 11:27:29 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable once CheckNamespace

namespace Newtonsoft.Json
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Text;
    using nGratis.Cop.Olympus.Contract;

    public static class SerializationExtensions
    {
        public static Stream SerializeManyToJson<T>(this IEnumerable<T> instances)
        {
            Guard
                .Require(instances, nameof(instances))
                .Is.Not.Null();

            return instances
                .ToArray()
                .SerializeToJson();
        }

        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static Stream SerializeToJson<T>(this T instance)
            where T : class
        {
            Guard
                .Require(instance, nameof(instance))
                .Is.Not.Null();

            var serializer = new JsonSerializer();
            var stream = new MemoryStream();

            using var streamWriter = new StreamWriter(stream, Encoding.UTF8, 4096, true);

            using var jsonWriter = new JsonTextWriter(streamWriter)
            {
                Formatting = Formatting.Indented,
                IndentChar = ' ',
                Indentation = 2
            };

            serializer.Serialize(jsonWriter, instance, typeof(T));
            jsonWriter.Flush();
            stream.Position = 0;

            return stream;
        }
    }
}