# Многопоточная сортировка
Основные этапы алгоритма:
1. Получить массив, который необходимо отсортировать (задается размер массива, заполняется случайными числами)
2. Далее пользователь задает количество потоков.
3. Исходный массив разбивается на части (двумя способами).
4. Каждый поток получает свою часть массива и сортирует его. Сортирует обычным «пузырьком», чтобы засечь разницу во времени.
5. Потоки отдают результаты своей работы в главный поток, и начинается «склейка» отсортированных
кусочков.
6. Результат и время работы отображается на консоли.

1 способ разбиения массива - просто по индексам (делится на равные части по количеству потоков). Реализация в файле Programm1.cs
2 способ разбиения массива - по значениям элементов массива. Вычисляется длина отрезка, на котором расположены все элементы массива, и этот отрезок делится на части в соответствии с количеством потоков. Каждый поток получает набор чисел, принадлежащих определенному отрезку. Реализация в файле Programm2.cs