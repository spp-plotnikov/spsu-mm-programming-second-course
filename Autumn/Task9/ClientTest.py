from math import *
from tkinter import *

f = open("ClientTest.txt")
points = dict()
for line in f:
    x, y = map(int, line.split(' '))
    points[x] = y

root = Tk()

canv = Canvas(root, width = 1000, height = 1000, bg = "white")
canv.create_line(100, 500, 100, 100, width = 1, arrow = LAST) 
canv.create_line(100, 500, 900, 500, width = 1, arrow = LAST) 

l = len(points.keys())
xscale = 800
yscale = 400
xdelta = xscale / (l + 1)
ydelta = points[l] / yscale
x = 100
for i in range(l):
	x += xdelta
	y = abs(points[i + 1] / ydelta - 500)
	canv.create_oval(x, y, x + 10, y + 10, fill = 'black')
	canv.create_line(x, y, x, 500, fill="red", dash=(2, 4))
	canv.create_line(x, y, 100, y, fill="red", dash=(2, 4))
	txt = '(' + str(i + 1) + ' , ' + str(points[i + 1]) + ')'
	canv.create_text(x, y, text = txt, font="Verdana 8",anchor="se",justify=CENTER,fill="blue")
	 
canv.pack()	
root.mainloop()
    
