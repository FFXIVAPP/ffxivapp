// FFXIVAPP
// DelegateCommand.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace FFXIVAPP.Classes.Commands
{
    public class DelegateCommand : ICommand
    {
        #region Constructors

        /// <summary>
        /// </summary>
        /// <param name="executeMethod"> </param>
        public DelegateCommand(Action executeMethod) : this(executeMethod, null, false)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="executeMethod"> </param>
        /// <param name="canExecuteMethod"> </param>
        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod) : this(executeMethod, canExecuteMethod, false)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="executeMethod"> </param>
        /// <param name="canExecuteMethod"> </param>
        /// <param name="isAutomaticRequeryDisabled"> </param>
        private DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod, bool isAutomaticRequeryDisabled)
        {
            if (executeMethod == null)
            {
                throw new ArgumentNullException("executeMethod");
            }
            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
            _isAutomaticRequeryDisabled = isAutomaticRequeryDisabled;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        private bool CanExecute()
        {
            return _canExecuteMethod == null || _canExecuteMethod();
        }

        /// <summary>
        /// </summary>
        private void Execute()
        {
            if (_executeMethod != null)
            {
                _executeMethod();
            }
        }

        /// <summary>
        /// </summary>
        public bool IsAutomaticRequeryDisabled
        {
            get { return _isAutomaticRequeryDisabled; }
            set
            {
                if (_isAutomaticRequeryDisabled == value)
                {
                    return;
                }
                if (value)
                {
                    CommandManagerHelper.RemoveHandlersFromRequerySuggested(_canExecuteChangedHandlers);
                }
                else
                {
                    CommandManagerHelper.AddHandlersToRequerySuggested(_canExecuteChangedHandlers);
                }
                _isAutomaticRequeryDisabled = value;
            }
        }

        /// <summary>
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        /// <summary>
        /// </summary>
        private void OnCanExecuteChanged()
        {
            CommandManagerHelper.CallWeakReferenceHandlers(_canExecuteChangedHandlers);
        }

        #endregion

        #region ICommand Members

        /// <summary>
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (!_isAutomaticRequeryDisabled)
                {
                    CommandManager.RequerySuggested += value;
                }
                CommandManagerHelper.AddWeakReferenceHandler(ref _canExecuteChangedHandlers, value, 2);
            }
            remove
            {
                if (!_isAutomaticRequeryDisabled)
                {
                    CommandManager.RequerySuggested -= value;
                }
                CommandManagerHelper.RemoveWeakReferenceHandler(_canExecuteChangedHandlers, value);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="parameter"> </param>
        /// <returns> </returns>
        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute();
        }

        /// <summary>
        /// </summary>
        /// <param name="parameter"> </param>
        void ICommand.Execute(object parameter)
        {
            Execute();
        }

        #endregion

        #region Data

        private readonly Action _executeMethod;
        private readonly Func<bool> _canExecuteMethod;
        private bool _isAutomaticRequeryDisabled;
        private List<WeakReference> _canExecuteChangedHandlers;

        #endregion
    }

    public class DelegateCommand<T> : ICommand
    {
        #region Constructors

        /// <summary>
        /// </summary>
        /// <param name="executeMethod"> </param>
        /// <param name="canExecuteMethod"> </param>
        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod) : this(executeMethod, canExecuteMethod, false)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="executeMethod"> </param>
        /// <param name="canExecuteMethod"> </param>
        /// <param name="isAutomaticRequeryDisabled"> </param>
        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod = null, bool isAutomaticRequeryDisabled = false)
        {
            if (executeMethod == null)
            {
                throw new ArgumentNullException("executeMethod");
            }
            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
            _isAutomaticRequeryDisabled = isAutomaticRequeryDisabled;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// </summary>
        /// <param name="parameter"> </param>
        /// <returns> </returns>
        private bool CanExecute(T parameter)
        {
            return _canExecuteMethod == null || _canExecuteMethod(parameter);
        }

        /// <summary>
        /// </summary>
        /// <param name="parameter"> </param>
        private void Execute(T parameter)
        {
            if (_executeMethod != null)
            {
                _executeMethod(parameter);
            }
        }

        /// <summary>
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        /// <summary>
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            CommandManagerHelper.CallWeakReferenceHandlers(_canExecuteChangedHandlers);
        }

        /// <summary>
        /// </summary>
        public bool IsAutomaticRequeryDisabled
        {
            get { return _isAutomaticRequeryDisabled; }
            set
            {
                if (_isAutomaticRequeryDisabled != value)
                {
                    if (value)
                    {
                        CommandManagerHelper.RemoveHandlersFromRequerySuggested(_canExecuteChangedHandlers);
                    }
                    else
                    {
                        CommandManagerHelper.AddHandlersToRequerySuggested(_canExecuteChangedHandlers);
                    }
                    _isAutomaticRequeryDisabled = value;
                }
            }
        }

        #endregion

        #region ICommand Members

        /// <summary>
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (!_isAutomaticRequeryDisabled)
                {
                    CommandManager.RequerySuggested += value;
                }
                CommandManagerHelper.AddWeakReferenceHandler(ref _canExecuteChangedHandlers, value, 2);
            }
            remove
            {
                if (!_isAutomaticRequeryDisabled)
                {
                    CommandManager.RequerySuggested -= value;
                }
                CommandManagerHelper.RemoveWeakReferenceHandler(_canExecuteChangedHandlers, value);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="parameter"> </param>
        /// <returns> </returns>
        bool ICommand.CanExecute(object parameter)
        {
            if (parameter == null && typeof (T).IsValueType)
            {
                return (_canExecuteMethod == null);
            }
            return CanExecute((T) parameter);
        }

        /// <summary>
        /// </summary>
        /// <param name="parameter"> </param>
        void ICommand.Execute(object parameter)
        {
            Execute((T) parameter);
        }

        #endregion

        #region Data

        private readonly Action<T> _executeMethod;
        private readonly Func<T, bool> _canExecuteMethod;
        private bool _isAutomaticRequeryDisabled;
        private List<WeakReference> _canExecuteChangedHandlers;

        #endregion
    }
}