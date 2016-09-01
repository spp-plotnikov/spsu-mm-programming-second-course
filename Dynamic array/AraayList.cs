using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrayList
{

    public class ArrayList<T> : IEnumerable
    {
        private T[] _array; //массив, куда записываются элементы
        private int count; //количество элементов массива

        //конструктор по умолчанию,  перегрузка, если сюда что то преедали
        //призывает конструктор с параметром
        public ArrayList()
            : this(4)
        {
        }
        //конструтктор с параметром: ошибка если длина меньше нуля
        public ArrayList(int length)
        {
            if (length < 0)
                throw new ArgumentException("length");
            _array = new T[length]; //создаем массив
            count = 0;
        }

        //расщиряет внутренний массив _array
        private void resize()
        {
            int index = _array.Length == 0 ? 4 : _array.Length * 2;
            /*в случае переполнения внутр. массива его размерность удваевается. 
             * если изначально разм равна нулю то при попытке добавить первый 
             * элемент размер увеличивается до 4*/
            T[] _newarray = new T[index];

            _array.CopyTo(_newarray, 0); //копируем из старого массива элементы в новый

            _array = _newarray; //переприсваивание ссылки

        }

        //полон ли внутренний массив
        public bool IsFull()
        {
            return count == _array.Length;
        }

        //добавление
        public void Add(T item) // добавляем item
        {
            if (this.IsFull()) //проверка на полноту
                this.resize(); //увеличивание в два раза

            _array[count++] = item; //на новое место, count  будет увеличен после

        }
        //вставка в середину коллекции
        public void Insert(T item, int index) //элемент и место в массиве
        {
            if (index > count)
                return;             //индекс больше колва элементов массива
            if (this.IsFull())
                this.resize();

            Array.Copy(_array, index, _array, index + 1, count - index); //копируем из _array начиная с индекса
            // в этот же массива  в индекс +1 количество элементов (count - index)
            _array[index] = item; //вставляем элемент

            count++;//увеличиваем колво элементов массива
        }
        //удаление элемента по индексу
        public void RemoveAt(int index)
        {

            if (index + 1 < count) //  если удаляется не последний элемент массива     
            {
                // Сдвиг массива на один элемент влево                                       
                Array.Copy(_array, index + 1, _array, index, count - index + 1);
            }


            count--; //уменьшаем количество элементов
            _array[count] = default(T); //последний элемент останется с каким то значением (из за копирования)

        }
        //удалять по объекту
        public bool Remove(T item)
        {
            for (int i = 0; i < count; i++)
                if (_array[i].Equals(item)) //проверка на равенство элемента в массиве
                {
                    RemoveAt(i);
                    return true; //элемент найден
                }
            return false;//элемент не найден
        }

        // Поиск индекса элемента
        public int IndexOf(T item)
        {
            for (int i = 0; i < count; i++)
            {
                if (_array[i].Equals(item))
                {
                    return i;
                }
            }
            return -1;  //не найден
        }


        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < count; i++)
                yield return _array[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}