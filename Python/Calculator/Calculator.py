from tkinter import *
import tkinter as tk

window = Tk()
window.geometry("350x500")
window.title("Rewa's Calculator")
window.configure(bg="#d1def0")
window.iconbitmap("calculator_icon.ico")

expression = ''
def press(n):
    global expression
    expression += str(n)
    equation.set(expression)

def equal():
    try:
        global expression
        total=str(eval(expression))
        equation.set(total)
        expression = ''
    except:
        equation.set('Error')
        expression = ''

def clear():
    global expression
    expression = ''
    equation.set(expression)

def backspace():
    global expression
    expression = expression.rstrip(expression[-1])
    equation.set(expression)



expression_frame=Frame(window,bg='#d1def0')
button_frame=Frame(window,bg='#d1def0')

expression_frame.pack()
button_frame.pack()

font_entry = ('ariel',20,'bold')
font_button = ('new times roman',12)

equation = StringVar()
equation.set('0')

equation_field = Entry(expression_frame,textvariable=equation,font=font_entry,justify='right')
equation_field.pack(ipadx=10,ipady=10,pady=20)

button1 = Button(button_frame,text='1',font=font_button,relief='ridge',bg='#3ea7ed',borderwidth=1,
                 width=8,height=3, command=lambda: press(1))
button2 = Button(button_frame,text='2',font=font_button,relief='ridge',bg='#3ea7ed',borderwidth=1,
                 width=8,height=3, command=lambda: press(2))
button3 = Button(button_frame,text='3',font=font_button,relief='ridge',bg='#3ea7ed',borderwidth=1,
                 width=8,height=3, command=lambda: press(3))
plus = Button(button_frame,text='+',font=font_button,relief='ridge',bg='#3ea7ed',borderwidth=1,
                 width=8,height=3, command=lambda: press('+'))
button4 = Button(button_frame,text='4',font=font_button,relief='ridge',bg='#3ea7ed',borderwidth=1,
                 width=8,height=3, command=lambda: press(4))
button5 = Button(button_frame,text='5',font=font_button,relief='ridge',bg='#3ea7ed',borderwidth=1,
                 width=8,height=3, command=lambda: press(5))
button6 = Button(button_frame,text='6',font=font_button,relief='ridge',bg='#3ea7ed',borderwidth=1,
                 width=8,height=3, command=lambda: press(6))
minus = Button(button_frame,text='-',font=font_button,relief='ridge',bg='#3ea7ed',borderwidth=1,
                 width=8,height=3, command=lambda: press('-'))
button7 = Button(button_frame,text='7',font=font_button,relief='ridge',bg='#3ea7ed',borderwidth=1,
                 width=8,height=3, command=lambda: press(7))
button8 = Button(button_frame,text='8',font=font_button,relief='ridge',bg='#3ea7ed',borderwidth=1,
                 width=8,height=3, command=lambda: press(8))
button9 = Button(button_frame,text='9',font=font_button,relief='ridge',bg='#3ea7ed',borderwidth=1,
                 width=8,height=3, command=lambda: press(9))
multiply= Button(button_frame,text='*',font=font_button,relief='ridge',bg='#3ea7ed',borderwidth=1,
                 width=8,height=3, command=lambda: press('*'))
button0 = Button(button_frame,text='0',font=font_button,relief='ridge',bg='#3ea7ed',borderwidth=1,
                 width=8,height=3, command=lambda: press(0))
decimal = Button(button_frame,text='.',font=font_button,relief='ridge',bg='#3ea7ed',borderwidth=1,
                 width=8,height=3, command=lambda: press('.'))
clear = Button(button_frame,text='CE',font=font_button,relief='ridge',bg='#3ea7ed',borderwidth=1,
                 width=8,height=3, command=clear)
divide = Button(button_frame,text='/',font=font_button,relief='ridge',bg='#3ea7ed',borderwidth=1,
                 width=8,height=3, command=lambda: press('/'))
equal = Button(button_frame,text='=',font=font_button,relief='ridge',bg='#3ea7ed',borderwidth=1,
                 width=8,height=3, command=equal)
clear_all = Button(button_frame,text='C',font=font_button,relief='ridge',bg='#3ea7ed',borderwidth=1,
                 width=8,height=3, command=clear)
backspace = Button(button_frame,text='<<',font=font_button,relief='ridge',bg='#3ea7ed',borderwidth=1,
                 width=8,height=3, command=backspace)

clear.grid(row=0,column=1)
clear_all.grid(row=0,column=2)
backspace.grid(row=0,column=3)
divide.grid(row=0,column=4)

button7.grid(row=1,column=1)
button8.grid(row=1,column=2)
button9.grid(row=1,column=3)
multiply.grid(row=1,column=4)

button4.grid(row=2,column=1)
button5.grid(row=2,column=2)
button6.grid(row=2,column=3)
minus.grid(row=2,column=4)

button1.grid(row=3,column=1)
button2.grid(row=3,column=2)
button3.grid(row=3,column=3)
plus.grid(row=3,column=4)

button0.grid(row=4,column=1,columnspan=2,sticky=tk.W+tk.E)
decimal.grid(row=4,column=3)
equal.grid(row=4,column=4)


window.mainloop()
