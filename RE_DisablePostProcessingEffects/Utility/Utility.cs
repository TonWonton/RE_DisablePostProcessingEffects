using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using REFrameworkNET;
using via;
using app;
using System;

namespace RE_DisablePostProcessingEffects
{
	internal static class Utility
	{
		//NativeSingleton
		internal static bool TryGetAndCacheNativeSingleton<T>([NotNullWhen(true)] ref NativeObject? nativeObject)
		{
			if (nativeObject != null) return true;
			else
			{
				NativeObject? thisNativeObject = API.GetNativeSingleton(typeof(T).FullName);
				if (thisNativeObject != null)
				{
					nativeObject = thisNativeObject;
					return true;
				}
			}

			return false;
		}

		//ManagedSingleton
		internal static bool TryGetAndCacheManagedSingleton<T>([NotNullWhen(true)] ref T? managedSingleton) where T : class
		{
			if (managedSingleton != null) return true;
			else
			{
				T? thisManagedSingleton = API.GetManagedSingletonT<T>();
				if (thisManagedSingleton != null)
				{
					managedSingleton = thisManagedSingleton;
					return true;
				}
			}

			return false;
		}

		//TypeDefinition
		internal static bool TryGetTypeDefinition(this NativeObject nativeObject, [NotNullWhen(true)] out TypeDefinition? typeDefinition)
		{
			TypeDefinition? thisTypeDefinition = nativeObject.GetTypeDefinition();
			if (thisTypeDefinition != null)
			{
				typeDefinition = thisTypeDefinition;
				return true;
			}

			typeDefinition = default;
			return false;
		}

		//MethodIndex
		internal static bool TryGetAndCacheMethodIndex(this NativeObject nativeObject, ref uint index, string name)
		{
			if (nativeObject.TryGetTypeDefinition(out TypeDefinition? typeDefinition))
			{
				index = typeDefinition.FindMethod(name).GetIndex();
				return true;
			}

			return false;
		}

		internal static bool TryGetAndCacheMethodIndex(this NativeObject nativeObject, ref uint index1, string name1, ref uint index2, string name2)
		{
			if (nativeObject.TryGetTypeDefinition(out TypeDefinition? typeDefinition))
			{
				index1 = typeDefinition.FindMethod(name1).GetIndex();
				index2 = typeDefinition.FindMethod(name2).GetIndex();
				return true;
			}

			return false;
		}

		internal static bool TryGetAndCacheMethodIndex(this NativeObject nativeObject, ref uint index1, string name1, ref uint index2, string name2, ref uint index3, string name3)
		{
			if (nativeObject.TryGetTypeDefinition(out TypeDefinition? typeDefinition))
			{
				index1 = typeDefinition.FindMethod(name1).GetIndex();
				index2 = typeDefinition.FindMethod(name2).GetIndex();
				index3 = typeDefinition.FindMethod(name3).GetIndex();
				return true;
			}

			return false;
		}

		internal static bool TryGetAndCacheMethodIndex(this NativeObject nativeObject, ref uint index1, string name1, ref uint index2, string name2, ref uint index3, string name3, ref uint index4, string name4)
		{
			if (nativeObject.TryGetTypeDefinition(out TypeDefinition? typeDefinition))
			{
				index1 = typeDefinition.FindMethod(name1).GetIndex();
				index2 = typeDefinition.FindMethod(name2).GetIndex();
				index3 = typeDefinition.FindMethod(name3).GetIndex();
				index4 = typeDefinition.FindMethod(name4).GetIndex();
				return true;
			}

			return false;
		}

		internal static bool TryGetAndCacheMethodIndex(this NativeObject nativeObject, ref uint index1, string name1, ref uint index2, string name2, ref uint index3, string name3, ref uint index4, string name4, ref uint index5, string name5)
		{
			if (nativeObject.TryGetTypeDefinition(out TypeDefinition? typeDefinition))
			{
				index1 = typeDefinition.FindMethod(name1).GetIndex();
				index2 = typeDefinition.FindMethod(name2).GetIndex();
				index3 = typeDefinition.FindMethod(name3).GetIndex();
				index4 = typeDefinition.FindMethod(name4).GetIndex();
				index5 = typeDefinition.FindMethod(name5).GetIndex();
				return true;
			}

			return false;
		}

		internal static bool TryGetAndCacheMethodIndex(this NativeObject nativeObject, ref uint index1, string name1, ref uint index2, string name2, ref uint index3, string name3, ref uint index4, string name4, ref uint index5, string name5, ref uint index6, string name6)
		{
			if (nativeObject.TryGetTypeDefinition(out TypeDefinition? typeDefinition))
			{
				index1 = typeDefinition.FindMethod(name1).GetIndex();
				index2 = typeDefinition.FindMethod(name2).GetIndex();
				index3 = typeDefinition.FindMethod(name3).GetIndex();
				index4 = typeDefinition.FindMethod(name4).GetIndex();
				index5 = typeDefinition.FindMethod(name5).GetIndex();
				index6 = typeDefinition.FindMethod(name6).GetIndex();
				return true;
			}

			return false;
		}

		internal static bool TryGetAndCacheMethodIndex(this NativeObject nativeObject, ref uint index1, string name1, ref uint index2, string name2, ref uint index3, string name3, ref uint index4, string name4, ref uint index5, string name5, ref uint index6, string name6, ref uint index7, string name7)
		{
			if (nativeObject.TryGetTypeDefinition(out TypeDefinition? typeDefinition))
			{
				index1 = typeDefinition.FindMethod(name1).GetIndex();
				index2 = typeDefinition.FindMethod(name2).GetIndex();
				index3 = typeDefinition.FindMethod(name3).GetIndex();
				index4 = typeDefinition.FindMethod(name4).GetIndex();
				index5 = typeDefinition.FindMethod(name5).GetIndex();
				index6 = typeDefinition.FindMethod(name6).GetIndex();
				index7 = typeDefinition.FindMethod(name7).GetIndex();
				return true;
			}

			return false;
		}

		//NativeObject
		internal static bool InvokeGetMethod_TryReturnValueAs<T>(this NativeObject nativeObject, uint index, object[]? args, ref object result, [NotNullWhen(true)] out T? returnValue) where T : class
		{
			nativeObject.HandleInvokeMember_Internal(index, args, ref result);

			T? thisReturnValue = ((ManagedObject)result).TryAs<T>();
			if (thisReturnValue != null)
			{
				returnValue = thisReturnValue;
				return true;
			}

			returnValue = default;
			return false;
		}

		internal static void InvokeSetMethod_Float32(this NativeObject nativeObject, uint index, object[] args, float arg1, ref object result)
		{
			//Prevent GC allocation from boxing
			Unsafe.Unbox<float>(args[0]) = arg1;
			nativeObject.HandleInvokeMember_Internal(index, args, ref result);
		}

		internal static void InvokeSetMethod_Bool(this NativeObject nativeObject, uint index, object[] args, bool arg1, ref object result)
		{
			//Prevent GC allocation from boxing
			Unsafe.Unbox<bool>(args[0]) = arg1;
			nativeObject.HandleInvokeMember_Internal(index, args, ref result);
		}
#if PRAGMATA
		//CameraGameObject
		internal static bool TryGetCameraGameObject([NotNullWhen(true)] ref CameraSystem? cameraSystem, [NotNullWhen(true)] out GameObject? cameraGameObject)
		{
			if (TryGetAndCacheManagedSingleton(ref cameraSystem))
			{
				GameObject? mainCameraGameObject = cameraSystem.getCameraObject(CameraDefine.Role.Main);
				if (mainCameraGameObject != null)
				{
					cameraGameObject = mainCameraGameObject;
					return true;
				}
			}

			cameraGameObject = default;
			return false;
		}
#endif
#if MHWILDS
		internal static bool TryGetCameraGameObject([NotNullWhen(true)] ref CameraManager? cameraManager, [NotNullWhen(true)] out GameObject? cameraGameObject)
		{
			if (TryGetAndCacheManagedSingleton(ref cameraManager))
			{
				Camera? primaryCamera = cameraManager.PrimaryCamera;
				if (primaryCamera != null)
				{
					GameObject? primaryCameraGameObject = primaryCamera.GameObject;
					if (primaryCameraGameObject != null)
					{
						cameraGameObject = primaryCameraGameObject;
						return true;
					}
				}
			}

			cameraGameObject = default;
			return false;
		}

#endif

		//Component
		internal static T? TryGetComponent<T>(this GameObject gameObject) where T : class
		{
			Component? componentFromGameObject = gameObject.getComponent(_System.Type.GetType(typeof(T).FullName));

			//Use operator and pattern matching just to be safe
			if (componentFromGameObject != null && componentFromGameObject is IObject componentIObject)
			{
				return componentIObject.TryAs<T>();
			}

			return default;
		}
	}
}
