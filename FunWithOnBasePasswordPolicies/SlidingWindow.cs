using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FunWithOnBasePasswordPolicies
{
    public class SlidingWindow
    {
        private int _size;
        private SWFunction _func;
        
        public SlidingWindow(int size, SWFunction func)
        {
            _size = size;
            _func = func;
        }

        public int Size
        {
            get { return _size;  }
        }

        public bool Execute(string password)
        {
            IEnumerable<char> array = password;
            // check the first window
            if (!_func(array.Take(Size)))
            {
                return false;
            }
            // check the remaining windows
            for (int i = Size ; i < array.Count(); i++)
            {
                if (!_func(array.Skip(i).Take(Size)))
                {
                    return false;
                }
                
            }
            return true;
        }
    }

    public interface ISlidingWindowFunction
    {
        bool Function(IEnumerable<char> characters);

    }

    public class MaximumRepeatedConsecutiveCharactersFunc : ISlidingWindowFunction
    {
        public bool Function(IEnumerable<char> characters)
        {
            char[] array = characters.ToArray();
            int reference = array[0]; 
            for(int i = 1; i < array.Length; i++)
            {
                if (array[i] == reference)
                {
                    return false;
                }
                else
                {
                    reference = array[i];
                }
            }
            return true;
        }
    }
    public delegate bool SWFunction(IEnumerable<char> characters);

    public class MaximumRepeatedConsecutiveCharacters
    {
        private SlidingWindow _window;

        public MaximumRepeatedConsecutiveCharacters(int size, SWFunction func)
        {
            _window = new SlidingWindow(size, func);
        }

        public bool Verify(string password)
        {
            return _window.Execute(password);
        }
    }
}
