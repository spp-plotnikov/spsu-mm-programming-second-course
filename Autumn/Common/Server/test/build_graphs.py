#!/usr/bin/python3

import json
import sys, os
import matplotlib.pyplot as plt
from math import *

with open('results.json') as data_file:    
    data = json.load(data_file)

# Data for plots
grx = [[], [], [], []]
gry = [[], [], [], []]

for elem in data:
    if elem is None:
        continue
    for i in range(4):
        grx[i].append(elem['count'])
    gry[0].append(elem['minTime'])
    gry[1].append(elem['maxTime'])
    gry[2].append(elem['avgTime'])
    gry[3].append(elem['medTime'])

# Plotting
plt.cla()
plt.plot(grx[0], gry[0], '-b.', label='Min time')
plt.plot(grx[1], gry[1], '-r.', label='Max time')
plt.plot(grx[2], gry[2], '-m.', label='Average time')
plt.plot(grx[3], gry[3], '-g.', label='Median time')
plt.xlabel('Client count')
plt.ylabel('Time, ms')
plt.legend(loc='upper left')
plt.savefig('plot.pdf')

