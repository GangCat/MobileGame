using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowButton : ButtonBase
{
    [SerializeField]
    private EArrowButtonType arrowType = EArrowButtonType.NONE;

    public EArrowButtonType ArrowType => arrowType;

}
