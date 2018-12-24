# Navi Faces
A very old project (created during Bachelor study time) integrated in an unfinished strategy game called **The Navigators** which was written 
by using the [Monogame](http://www.monogame.net/) framework. The architecture was too complicated
in comparison to the MVC-pattern similar architectures since 2017/18 used in other projects.
Also, the description is currently only available in German. It is about a new language which allows you
to design multilingual, resolution independent menus/huds in computer games very fast/time efficient due to a hard separation of logic
and design e.g. a fully working main-menu with four buttons written in the Faces-language:

```ruby
import(Windows.Main);

Define:
color = #color=Button;
highlight = #color=ButtonHighlight;
sound = #sound=Widget;
text = #post#db;
textColor = #color=TextBody;

Create:
btn{New, Controls, Credits, Exit}(color, highlight, sound, text, textColor);

Link:
btnNew[N]->MapLoading;
btn{Controls, Credits}->#post;
btnExit[E]->#exit;

Add:
(btn{New, Controls, Credits}, 50.0, 39.2[+10.0], 36.5, 7.29);
(btnExit, y = 83.3);
```

**corresponding generated menu/surface:**
<img src="https://github.com/AlexanderMattheis/navi-faces-CANCELED/blob/master/Preview/Capture.PNG">

## Developer-Funktionen
Funktionen, denen der Entwickler eine bestimmte Funktion zugeteilt hat. Diesen Funktionen sind unterschiedliche Rechte zu Teil. Daher müssen sie unterschiedlich gehandhabt werden. Beispielsweise erlaubt die ``Add``-Funktion nur die Übergabe von Widgets ans Surface, während in der ``Define``-Funktion lediglich Variablen definiert werden können.

### 1. Define 
Definition von Variablen, die für die Objekte/Widgets benötigt werden.

#### Beispiele:
```ruby
Define:
color = #color=Button;
highlight = #color=ButtonHighlight;
pivot = (50.0, 50.0);
sound = #sound=Widget;
```
**Hint:** Default-Werte werden aus Banken eines bestimmten Typs entnommen z.B. wird hier für ``highlight`` und ``button``, 
die ``color``-Bank und für die Sounds, die ``sound``-Bank verwendet.

Die Deklaration von Variablen sollte nach Konvention in alphabetischer Reihenfolge bzgl. des Variablentyps erfolgen. 
Man könnte die Variablen mit den primitiven Variablentypen gängiger Programmiersprachen vergleichen.

### 2. Create: 
Zum Erstellen von Objekten/Widgets.

#### Beispiele:
```ruby
Create:
btn{New, Controls, Credits, Exit}(color, highlight, sound, text, textColor);
lblChooseAMap(text, textColor);
```

wobei: <br>
``lbl``: Datentyp <br>
``ChooseAMap``: Objektname

Dient der Erstellung sichtbarer Oberflächenobjekte ``img..., btn...``. Variablen können, aber müssen nicht an die Objekte übergeben werden. Variablen ``color, highlight, sound, ...`` werden in alphabetischer Reihenfolge übergeben.

##### Objekt-Arrays
```ruby
btn{New, Credits, ...}([parameter])
```

### 3. Link
Zum Verlinken von Objekten.

#### Beispiele:
```ruby
Link:
btnNew[N]->MapLoading;
btn{Controls, Credits}->#post;
btnExit[E]->#exit;
```

Zuvor erstellte Objekte werden auf andere Surfaces/Menüs verwiesen und Funktionen zugeteilt. Objekte werden mit dem ``->``- Operator verlinkt. Das heißt, die darauffolgende Funktion oder das jeweilige Surface/Menü wird aufgerufen.

##### Kommandos
``#post``: nimmt den Objektnamen als Surfacenamen und verlinkt darauf <br>
``#big``: der String an den ``#big`` angehängt wird, wird in Großbuchstaben geschrieben <br>
``#exit``: verlässt die Anwendung

##### Stellvertreter/Shortcuts
```
btnNew[N]->MapLoading;
```
bedeutet die Taste ``N`` ist ein Stellvertreter/Shortcut beim ``MapLoading``.
Heißt, über die Taste ``N`` gelangt man in das ``MapLoading``-Menü.

### 4. Add 
Zum Hinzufügen der Objekte zum Surface.

#### Beispiele:
```ruby
Add:
(btn{New, Controls, Credits}, 50.0, 39.2[+10.0], 36.5, 7.29);
(btnExit, y = 83.3);
```

## Parameterübergabe
Die Parameterübergabe findet über die Parameterklammern ``(`` und ``)`` statt. 
Es wird jeweils zur nächsthöheren Systemebene übergeben.

### Ebenen
1. Screen <br>                                                                                                                                                   
2. Surface <br>								                  
3. Widget (``img..., btn...``) <br>

also: <br>
Variablen -> Widget -> Surface -> Screen

#### Beispiele:
```ruby
Create:
btn{New, Controls, Credits, Exit}(color, highlight, sound, text, textColor);

Add:
(btnNew, 50.0, 39.3, 36.5, 7.29);
```

Somit werden Variablen ``color, highlight, sound, text, textColor`` an Objekte vom Typ ``btn`` übergeben
und der eine ``btn`` mit den gegebenen Parametern ``50.0, 39.3, 36.5, 7.29`` wird ans Surface übergeben.

## Variablen
Variablen wird der Wert auf der rechten Seite (also nach dem ``=``) zugewiesen und Variablen werden Objekten übergeben.
Sie können auch aufgezählt werden
```ruby
image<0> = #image=DefaultLogo;
image<1> = GUI.Icons.arrow_left;
...
```

**Hinweis:** Eigene Variablen können nicht deklariert werden! Alle verfügbaren Variablen sind unten drunter aufgelistet.

### Buttons:
- color
- font
- image
- highlight
- pivot
- shinyImage
- text
- textAligment
- textColor
- textScale
- textPivot

### Images:
- color
- image
- pivot

### Labels:
- color
- image
- font
- pivot
- text
- textAligment
- textColor
- textScale
- textPivot

### System:
- alignment
- dimensions
