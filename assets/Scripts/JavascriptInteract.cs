using System.Runtime.InteropServices;
using UnityEngine;

namespace DefaultNamespace
{
    public class JavascriptInteract : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void Hello();

        [DllImport("__Internal")]
        private static extern void HelloString(string str);

        [DllImport("__Internal")]
        private static extern void PrintFloatArray(float[] array, int size);

        [DllImport("__Internal")]
        private static extern int AddNumbers(int x, int y);

        [DllImport("__Internal")]
        private static extern string StringReturnValueFunction();

        [DllImport("__Internal")]
        private static extern void BindWebGLTexture(int texture);

        void Start() {
#if UNITY_WEBGL && !UNITY_EDITOR
            Hello();
        
            HelloString("This is a string.");
        
            float[] myArray = new float[10];
            PrintFloatArray(myArray, myArray.Length);
        
            int result = AddNumbers(5, 7);
            Debug.Log(result);
        
            Debug.Log(StringReturnValueFunction());
        
            var texture = new Texture2D(0, 0, TextureFormat.ARGB32, false);
            BindWebGLTexture(texture.GetNativeTextureID());
#endif
        }

        public void VoidFunction()
        {
            HelloString("Void function called");
        }

        public void IntFunction(int i)
        {
            HelloString("Input: " + i);
        }

        public void StringFunction(string s)
        {
            HelloString("Input: " + s);
        }
    }
}