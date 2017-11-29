TagsCloudContainer
======

Реализовано:
* Консольный интерфейс
* Входные форматы: txt и docx 
* (пока по одному слову в строке)
* Выходные форматы: png, bmp, gif, jpeg
* Возможность задать цвета, шрифт и размер изображения

Пример использования:
```
TagsCloudContainer.Cli.exe 
    -i example.docx 
    --docformat docx 
    -o out.jpg 
    --imgformat jpeg 
    -h 500 -w 500 
    --bgcolor Yellow 
    --txtcolor Red 
    --font Calibri
```