﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AweFileChooser.cs" company="nGratis">
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
// <creation_timestamp>Sunday, 9 April 2017 2:25:09 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.Cop.Olympus.Wpf
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class AweFileChooser : ContentControl
    {
        public static readonly DependencyProperty SelectedFilePathProperty = DependencyProperty.Register(
            nameof(AweFileChooser.SelectedFilePath),
            typeof(string),
            typeof(AweFileChooser),
            new PropertyMetadata(null));

        public static readonly DependencyProperty AuxiliaryTextProperty = DependencyProperty.Register(
            nameof(AweFileChooser.AuxiliaryText),
            typeof(string),
            typeof(AweFileChooser),
            new PropertyMetadata("<AUX>"));

        public static readonly DependencyProperty AuxiliaryCommandProperty = DependencyProperty.Register(
            nameof(AweFileChooser.AuxiliaryCommand),
            typeof(ICommand),
            typeof(AweFileChooser),
            new PropertyMetadata(null));

        public string SelectedFilePath
        {
            get => (string)this.GetValue(AweFileChooser.SelectedFilePathProperty);
            set => this.SetValue(AweFileChooser.SelectedFilePathProperty, value);
        }

        public string AuxiliaryText
        {
            get => (string)this.GetValue(AweFileChooser.AuxiliaryTextProperty);
            set => this.SetValue(AweFileChooser.AuxiliaryTextProperty, value);
        }

        public ICommand AuxiliaryCommand
        {
            get => (ICommand)this.GetValue(AweFileChooser.AuxiliaryCommandProperty);
            set => this.SetValue(AweFileChooser.AuxiliaryCommandProperty, value);
        }
    }
}