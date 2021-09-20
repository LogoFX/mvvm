using System;
using LogoFX.Client.Mvvm.Model.Contracts;
using LogoFX.Core;

namespace LogoFX.Client.Mvvm.Model
{
    partial class EditableModel<T>
    {
        /// <summary>
        /// This class represents an editable model which supports undo and redo operations.
        /// </summary>   
        public class WithUndoRedo : EditableModel<T>, IUndoRedo
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="EditableModel{T}.WithUndoRedo"/> class.
            /// </summary>
            public WithUndoRedo()
            {
                SubscribeToUndoRedoHistoryEvents();
            }            

            /// <inheritdoc />
            public bool CanUndo => _history.CanUndo;

            /// <inheritdoc />
            public bool CanRedo => _history.CanRedo;

            /// <inheritdoc />
            public void Undo()
            {
                if (_history.CanUndo)
                {
                    _history.Undo();                   
                }
                if (_history.CanUndo == false)
                {
                    ClearDirty();
                }
            }

            /// <inheritdoc />
            public void Redo()
            {
                if (_history.CanRedo)
                {
                    _history.Redo();                   
                }
            }

            /// <inheritdoc />
            public override void MakeDirty()
            {
                if ((OwnDirty && CanCancelChanges) == false)
                {
                    OwnDirty = true;    
                }                
                AddToHistory();
            }

            private void SubscribeToUndoRedoHistoryEvents()
            {
                EventHandler undoStrongHandler = HistoryOnUndoStackChanged;
                _history.UndoStackChanged += WeakDelegate.From(undoStrongHandler);
                EventHandler redoStrongHandler = HistoryOnRedoStackChanged;
                _history.RedoStackChanged += WeakDelegate.From(redoStrongHandler);
            }

            private void HistoryOnUndoStackChanged(object sender, EventArgs eventArgs)
            {
                NotifyOfPropertyChange(() => CanUndo);
            }

            private void HistoryOnRedoStackChanged(object sender, EventArgs eventArgs)
            {
                NotifyOfPropertyChange(() => CanRedo);
            }
        }
    }
}