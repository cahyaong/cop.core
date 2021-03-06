﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReactiveScreen.cs" company="nGratis">
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
// <creation_timestamp>Friday, August 28, 2020 5:48:47 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.Cop.Olympus.Wpf.Glue
{
    using System;
    using System.Reactive.Concurrency;
    using System.Threading;
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using ReactiveUI;

    public class ReactiveScreen : ReactiveViewAware, Caliburn.Micro.IScreen, IChild
    {
        private string _displayName;

        private bool _isInitialized;

        private bool _isActive;

        private bool _isNotifying;

        private object _parent;

        public string DisplayName
        {
            get => this._displayName;
            set => this.RaiseAndSetIfChanged(ref this._displayName, value);
        }

        public bool IsInitialized
        {
            get => this._isInitialized;
            private set => this.RaiseAndSetIfChanged(ref this._isInitialized, value);
        }

        public bool IsActive
        {
            get => this._isActive;
            private set => this.RaiseAndSetIfChanged(ref this._isActive, value);
        }

        public bool IsNotifying
        {
            get => this._isNotifying;
            set => this.RaiseAndSetIfChanged(ref this._isNotifying, value);
        }

        public object Parent
        {
            get => this._parent;
            set => this.RaiseAndSetIfChanged(ref this._parent, value);
        }

        public event EventHandler<DeactivationEventArgs> AttemptingDeactivation;

        public event EventHandler<DeactivationEventArgs> Deactivated;

        public async Task ActivateAsync(CancellationToken cancellationToken)
        {
            if (this.IsActive)
            {
                return;
            }

            if (!this.IsInitialized)
            {
                await this.InitializeAsync(cancellationToken);
                this.IsInitialized = true;
            }

            await this.ActivateCoreAsync(cancellationToken);
            this.IsActive = true;

            this.RaiseActivated();
        }

        public event EventHandler<ActivationEventArgs> Activated;

        public async Task DeactivateAsync(bool isClosed, CancellationToken cancellationToken)
        {
            var shouldDeactivate =
                this.IsActive ||
                this.IsInitialized && isClosed;

            if (shouldDeactivate)
            {
                this.RaiseDeactivating(isClosed);

                await this.DeactivateCoreAsync(isClosed, cancellationToken);

                this.IsActive = false;
                this.RaisedDeactivated(isClosed);

                if (isClosed)
                {
                    this.Views.Clear();
                }
            }
        }

        public virtual async Task<bool> CanCloseAsync(CancellationToken cancellationToken)
        {
            return await Task.FromResult(true);
        }

        public virtual async Task TryCloseAsync(bool? dialogResult)
        {
            if (this.Parent is IConductor conductor)
            {
                await conductor.CloseItemAsync(this, CancellationToken.None);
            }

            var closeAsync = PlatformProvider.Current.GetViewCloseAction(this, this.Views.Values, dialogResult);

            RxApp.MainThreadScheduler.Schedule(async () => await closeAsync(CancellationToken.None));
        }

        public void Refresh()
        {
            this.RaisePropertyChanged(string.Empty);
        }

        public void NotifyOfPropertyChange(string propertyName)
        {
            this.RaisePropertyChanged(propertyName);
        }

        protected virtual async Task InitializeAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        protected virtual async Task ActivateCoreAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        protected virtual async Task DeactivateCoreAsync(bool isClosed, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        private void RaiseActivated()
        {
            var args = new ActivationEventArgs
            {
                WasInitialized = this.IsInitialized
            };

            this.Activated?.Invoke(this, args);
        }

        private void RaiseDeactivating(bool isClosed)
        {
            var args = new DeactivationEventArgs
            {
                WasClosed = isClosed
            };

            this.AttemptingDeactivation?.Invoke(this, args);
        }

        private void RaisedDeactivated(bool isClosed)
        {
            var args = new DeactivationEventArgs
            {
                WasClosed = isClosed
            };

            this.Deactivated?.Invoke(this, args);
        }
    }
}