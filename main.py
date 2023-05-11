# This is a sample Python script.
import math

# Press Shift+F10 to execute it or replace it with your code.
# Press Double Shift to search everywhere for classes, files, tool windows, actions, and settings.


def targil():
    option = int(input("Hello, if you want rectangle press 1 "
                       " \nif you want triangle press 2\nif you want to exit press 3\n"))
    while(option != 3):
        width = int(input("enter the width of the tower"))
        length = int(input("enter the length of the tower"))
        if (option == 1):
            rectangle(width,length)
        elif (option == 2):
            triangle(width,length)
        option = int(input("Hello, if you want rectangle press 1 "
                           " \nif you want triangle press 2\nif you want to exit press 3\n"))


def triangle(width,length):
    option_triangle = int(input("press 1 for perimeter of a  triangle \npress 2 for print the triangle"))
    if (option_triangle == 1):
        print("the triangle's circumference", math.sqrt(pow(width / 2, 2) + pow(length, 2))*2+width)
    else:
        if ((width % 2 == 0) or (width >= length * 2) or (width==3 and  length!=2 )or width==1):
            print("It is not possible to print the given triangle")
        else:
            odd = int(((width) / 2) - 1)
            if(odd != 0):
                help1 = int((length - 2) / odd)
                help2 = int((length - 2) / odd)
                if (odd * help1 != (length - 2)):
                    help1 += (length - 2 - (odd * help1))
                    flag = 1
            flag = 0
            num = 3
            space = odd + 1
            print(' ' * space + '*')
            space -= 1
            for i in range(odd):
                for j in range(help1):
                    print(' ' * space + '*' * num)
                num += 2
                space -= 1
                if (flag == 1 and i == 0):
                    help1 -= (length - 2 - (odd * help2))
            print('*' * width)

def rectangle(width,length):
    if (abs(width - length) > 5):
        print("the area of the rectangle", width * length)
    else:
        print("The scope of the rectangle", (width * 2) + (length * 2))


def print_hi(name):
    # Use a breakpoint in the code line below to debug your script.
    print(f'Hi, {name}')  # Press Ctrl+F8 to toggle the breakpoint.


# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    print_hi('PyCharm')
    targil()

# See PyCharm help at https://www.jetbrains.com/help/pycharm/
