﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockExtensions.Stream.cs" company="nGratis">
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
// <creation_timestamp>Sunday, 1 April 2018 4:26:50 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable once CheckNamespace

namespace Moq
{
    using System.IO;

    public static partial class MockExtensions
    {
        public static Mock<Stream> WithReadable(this Mock<Stream> mockStream)
        {
            mockStream
                .SetupGet(mock => mock.CanRead)
                .Returns(true)
                .Verifiable();

            return mockStream;
        }

        public static Mock<Stream> WithWritable(this Mock<Stream> mockStream)
        {
            mockStream
                .SetupGet(mock => mock.CanWrite)
                .Returns(true)
                .Verifiable();

            return mockStream;
        }

        public static Mock<Stream> WithContent(this Mock<Stream> mockStream, string content)
        {
            mockStream
                .SetupGet(mock => mock.Length)
                .Returns(content.Length)
                .Verifiable();

            return mockStream;
        }
    }
}