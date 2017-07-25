using System;
using System.Collections.Generic;
using System.Text;

namespace UGCXamarin.Utils.Json.Extensions {
    static class StackExtension {
        public static T[] ToReverseArray<T>(this Stack<T> stack) {
            T[] reversalArray = stack.ToArray();
            Array.Reverse(reversalArray);
            return reversalArray;
        }
    }
}
