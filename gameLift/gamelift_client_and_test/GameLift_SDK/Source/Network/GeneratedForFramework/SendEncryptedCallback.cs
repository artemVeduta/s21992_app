//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 4.0.0
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace GameLiftRealtimeNative {

public class SendEncryptedCallback : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal SendEncryptedCallback(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(SendEncryptedCallback obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~SendEncryptedCallback() {
    Dispose(false);
  }

  public void Dispose() {
    Dispose(true);
    global::System.GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool disposing) {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          GameLiftRealtimeNativePINVOKE.delete_SendEncryptedCallback(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
    }
  }

  public virtual void sendEncrypted(SWIGTYPE_p_unsigned_char buf, uint len) {
    GameLiftRealtimeNativePINVOKE.SendEncryptedCallback_sendEncrypted(swigCPtr, SWIGTYPE_p_unsigned_char.getCPtr(buf), len);
  }

  public SendEncryptedCallback() : this(GameLiftRealtimeNativePINVOKE.new_SendEncryptedCallback(), true) {
    SwigDirectorConnect();
  }

  private void SwigDirectorConnect() {
    global::System.IntPtr swigDelegate0gcHandlePtr = global::System.IntPtr.Zero;
    if (SwigDerivedClassHasMethod("sendEncrypted", swigMethodTypes0)) {
      swigDelegate0 = new SwigDelegateSendEncryptedCallback_0(SwigDirectorsendEncrypted);
      swigDelegate0dispatcher = new SwigDelegateSendEncryptedCallback_0_Dispatcher(SwigDirectorsendEncrypted_Dispatcher);
      global::System.Runtime.InteropServices.GCHandle swigDelegate0gcHandle = global::System.Runtime.InteropServices.GCHandle.Alloc(swigDelegate0, global::System.Runtime.InteropServices.GCHandleType.Weak);
      swigDelegate0gcHandlePtr = global::System.Runtime.InteropServices.GCHandle.ToIntPtr(swigDelegate0gcHandle);
    }
    GameLiftRealtimeNativePINVOKE.SendEncryptedCallback_director_connect(swigCPtr, swigDelegate0dispatcher, swigDelegate0gcHandlePtr);
  }

  private bool SwigDerivedClassHasMethod(string methodName, global::System.Type[] methodTypes) {
    global::System.Reflection.MethodInfo methodInfo = this.GetType().GetMethod(methodName, global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.NonPublic | global::System.Reflection.BindingFlags.Instance, null, methodTypes, null);
    bool hasDerivedMethod = methodInfo.DeclaringType.IsSubclassOf(typeof(SendEncryptedCallback));
    return hasDerivedMethod;
  }

  private void SwigDirectorsendEncrypted(global::System.IntPtr buf, uint len) {
    sendEncrypted((buf == global::System.IntPtr.Zero) ? null : new SWIGTYPE_p_unsigned_char(buf, false), len);
  }

  [GameLiftRealtimeNativePINVOKE.MonoPInvokeCallback(typeof(SwigDelegateSendEncryptedCallback_0_Dispatcher))]
  private static void SwigDirectorsendEncrypted_Dispatcher(global::System.IntPtr swigDelegateSendEncryptedCallback_0_Handle, global::System.IntPtr buf, uint len) {
    global::System.Runtime.InteropServices.GCHandle gcHandle = global::System.Runtime.InteropServices.GCHandle.FromIntPtr(swigDelegateSendEncryptedCallback_0_Handle);
    SwigDelegateSendEncryptedCallback_0 delegateSwigDelegateSendEncryptedCallback_0 = (SwigDelegateSendEncryptedCallback_0) gcHandle.Target;
delegateSwigDelegateSendEncryptedCallback_0(buf, len);
  }

  public delegate void SwigDelegateSendEncryptedCallback_0(global::System.IntPtr buf, uint len);

  private SwigDelegateSendEncryptedCallback_0 swigDelegate0;

  public delegate void SwigDelegateSendEncryptedCallback_0_Dispatcher(global::System.IntPtr swigDelegateSendEncryptedCallback_0_Handle, global::System.IntPtr buf, uint len);

  private SwigDelegateSendEncryptedCallback_0_Dispatcher swigDelegate0dispatcher;

  private static global::System.Type[] swigMethodTypes0 = new global::System.Type[] { typeof(SWIGTYPE_p_unsigned_char), typeof(uint) };
}

}