using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphsPrj
{
    public enum LayerType
    {
        None,
        LayerBackImage,//CT,MRT etc.
        LayerContour,//Organ,GTV etc.
        LayerEquipment,//MLC etc.
        LayerDose,
        LayerPOI,
        LayerGrid,
        LayerAxis,
        LayerMessage,
        LayerRuler,
        LayerOperator
    }
}
