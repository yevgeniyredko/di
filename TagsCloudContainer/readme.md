TagsCloudContainer
======

#### Реализовано:
* Консольный интерфейс
* Входные форматы: __txt__ и __docx__ 
* Поддержка произвольных текстов
* Возможность задать свои скучные слова
* Выходные форматы: __png__, __bmp__, __gif__, __jpeg__
* Возможность задать цвета, шрифт и размер изображения
* Алгоритмы расцветки слов:
  * __Один цвет__ - все слова имеют один текст 
  * __Градиент__ - чем меньше слово, тем прозрачнее его цвет
  * __Случайные цвета__
* Алгоритмы масштабирования слов:
  * __Линейный__
  * __Нелинейный__ 

Примеры использования
======

#### Расцветка одним цветом, линейное масштабирование
```
TagsCloudContainer.Cli.exe 
    -i gogol.txt 
    -o out_linear_single.png 
    -h 600 -w 600 
    --bgcolor White 
    --textcolor Black
```
![out_linear_single](/TagsCloudContainer/examples/out_linear_single.png)

#### Расцветка градиентом, линейное масштабирование
```
TagsCloudContainer.Cli.exe 
    -i gogol.txt 
    -o out_linear_gradient.png 
    -h 600 -w 600 
    --bgcolor White 
    --textcolor Black 
    --textcolormode gradient
```
![out_linear_gradient](/TagsCloudContainer/examples/out_linear_gradient.png)

#### Случайная расцветка, нелинейное масштабирование
```
TagsCloudContainer.Cli.exe 
    -i gogol.txt 
    -o out_nonlinear_random.png 
    -h 600 -w 600 
    --bgcolor White 
    --textcolormode random 
    --scale nonlinear
```
![out_nonlinear_random](/TagsCloudContainer/examples/out_nonlinear_random.png)

#### Обычная расцветка, нелинейное масштабирование, исключены слова "быть", "сказать", "который"
```
TagsCloudContainer.Cli.exe 
    -i gogol.txt 
    -o out_b_w.png 
    -h 600 -w 600 
    --scale nonlinear 
    --bgcolor White 
    --textcolor Black 
    --boring быть сказать который
```
![out_b_w](/TagsCloudContainer/examples/out_b_w.png)