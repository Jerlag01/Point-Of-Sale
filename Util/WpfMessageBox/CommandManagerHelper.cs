using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Util.WpfMessageBox
{
    internal class CommandManagerHelper
    {
        internal static Action<List<WeakReference>> CallWeakReferenceHandlers = (Action<List<WeakReference>>)(x =>
        {
            if (x == null)
                return;
            EventHandler[] eventHandlerArray = new EventHandler[x.Count];
            int index1 = 0;
            for (int index2 = x.Count - 1; index2 >= 0; --index2)
            {
                if (!(x[index2].Target is EventHandler target))
                {
                    x.RemoveAt(index2);
                }
                else
                {
                    eventHandlerArray[index1] = target;
                    ++index1;
                }
            }
            for (int index2 = 0; index2 < index1; ++index2)
                eventHandlerArray[index2]((object)null, EventArgs.Empty);
        });
        internal static Action<List<WeakReference>> AddHandlersToRequerySuggested = (Action<List<WeakReference>>)(x => x?.ForEach((Action<WeakReference>)(y =>
        {
            if (!(y.Target is EventHandler target))
                return;
            CommandManager.RequerySuggested += target;
        })));
        internal static Action<List<WeakReference>> RemoveHandlersFromRequerySuggested = (Action<List<WeakReference>>)(x => x?.ForEach((Action<WeakReference>)(y =>
        {
            if (!(y.Target is EventHandler target))
                return;
            CommandManager.RequerySuggested -= target;
        })));
        internal static Action<List<WeakReference>, EventHandler> RemoveWeakReferenceHandler = (Action<List<WeakReference>, EventHandler>)((x, y) =>
        {
            if (x == null)
                return;
            for (int index = x.Count - 1; index >= 0; --index)
            {
                if (!(x[index].Target is EventHandler target) || target == y)
                    x.RemoveAt(index);
            }
        });

        internal static void AddWeakReferenceHandler(
          ref List<WeakReference> handlers,
          EventHandler handler)
        {
            CommandManagerHelper.AddWeakReferenceHandler(ref handlers, handler, -1);
        }

        internal static void AddWeakReferenceHandler(
          ref List<WeakReference> handlers,
          EventHandler handler,
          int defaultListSize)
        {
            if (handlers == null)
                handlers = defaultListSize > 0 ? new List<WeakReference>(defaultListSize) : new List<WeakReference>();
            handlers.Add(new WeakReference((object)handler));
        }
    }
}