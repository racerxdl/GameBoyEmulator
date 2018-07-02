#!/usr/bin/env python


def ApplyTemplate(tpl, vars={}):
    o = tpl
    for k in vars:
        o = o.replace("{%s}" %k.upper() , str(vars[k]))
    return o
