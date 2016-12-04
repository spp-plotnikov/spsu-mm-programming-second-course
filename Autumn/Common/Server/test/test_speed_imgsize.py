#!/usr/bin/env python3

import json
import sys, os
import matplotlib.pyplot as plt
import time
from math import *
from PIL import Image
from numpy import random
import subprocess

# Data for plots
grx = [[], [], [], []]
gry = [[], [], [], []]

for size in range(10000, 2100000, 100000):
    print('Testing for size =', size, '...')
    
    width = height = ceil(sqrt(size))
    Z = random.rand(width, height, 3) * 255
    img = Image.fromarray(Z.astype('uint8')).convert('RGBA')
    img.save('.tmp_img.png')

    cmd = ['java', '-jar', 'test.jar', '.tmp_img.png']
    subprocess.Popen(cmd).wait()
    
    with open('.tmp_res.json') as data_file:    
        data = json.load(data_file)
    os.remove('.tmp_res.json')
    os.remove('.tmp_img.png')
    
    for i in range(4):
        grx[i].append(size)
    gry[0].append(data['minTime'])
    gry[1].append(data['maxTime'])
    gry[2].append(data['avgTime'])
    gry[3].append(data['medTime'])

# Plotting
plt.cla()
plt.plot(grx[0], gry[0], '-b.', label='Min time')
plt.plot(grx[1], gry[1], '-r.', label='Max time')
plt.plot(grx[2], gry[2], '-m.', label='Average time')
plt.plot(grx[3], gry[3], '-g.', label='Median time')
plt.xlabel('Pixel count')
plt.ylabel('Time, ms')
plt.legend(loc='upper left')
plt.savefig('plot2.pdf')
