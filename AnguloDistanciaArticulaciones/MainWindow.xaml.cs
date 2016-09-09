using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
namespace AnguloDistanciaArticulaciones
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor MiKinect;   
        public MainWindow()
        {
            InitializeComponent();
        }
        //Evento para calcular distancias:
        private float ObtenerDistancia(Joint firstJoint, Joint secondJoint)
        {
            float Distancia_X = firstJoint.Position.X - secondJoint.Position.X;
            float Distancia_Y = firstJoint.Position.Y - secondJoint.Position.Y;
            float Distancia_Z = firstJoint.Position.Z - secondJoint.Position.Z;

            return (float)Math.Sqrt(Math.Pow(Distancia_X, 2) + Math.Pow(Distancia_Y, 2) + Math.Pow(Distancia_Z, 2));                //Realiza el calculo de la distancia entre articulaciones
        }
        //Evento para calcular angulos:
        private double Angulos(Joint j1, Joint j2, Joint j3)
        {
            double Angulo = 0;
            double shrhX = j1.Position.X - j2.Position.X;
            double shrhY = j1.Position.Y - j2.Position.Y;
            double shrhZ = j1.Position.Z - j2.Position.Z;
            double hsl = vectorNorm(shrhX, shrhY, shrhZ);
            double unrhX = j3.Position.X - j2.Position.X;
            double unrhY = j3.Position.Y - j2.Position.Y;
            double unrhZ = j3.Position.Z - j2.Position.Z;
            double hul = vectorNorm(unrhX, unrhY, unrhZ);
            double mhshu = shrhX * unrhX + shrhY * unrhY + shrhZ * unrhZ;
            double x = mhshu / (hul * hsl);

            if (x != Double.NaN)
            {
                if (-1 <= x && x <= 1)
                {
                    double angleRAd = Math.Acos(x);
                    Angulo = angleRAd * (180.0 / Math.PI);
                }
                else
                    Angulo = 0;
            }
            else
                Angulo = 0;

            return Angulo;
        }
        //Muestra:
        private static double vectorNorm(double x, double y, double z)
        {

            return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));

        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(KinectSensor.KinectSensors.Count ==0)
            {
                MessageBox.Show("No se ha detectado ningun Kinect");
                Application.Current.Shutdown();
            }
            MiKinect = KinectSensor.KinectSensors.FirstOrDefault();
            try
            {
                MiKinect.SkeletonStream.Enable();
                MiKinect.Start();
            }catch
            {
                MessageBox.Show("La inicializacion del kinect fallo");
                Application.Current.Shutdown();
            }
            MiKinect.SkeletonFrameReady += MiKinect_SkeletonFrameReady;
        }

        void MiKinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            canvasesqueleto.Children.Clear();
            Skeleton[] esqueletos = null;
            using (SkeletonFrame frameesqueleto=e.OpenSkeletonFrame() ) 
            {
                if(frameesqueleto !=null)
                {
                    esqueletos= new Skeleton [frameesqueleto.SkeletonArrayLength];
                    frameesqueleto.CopySkeletonDataTo(esqueletos);
                }
            }
            if (esqueletos == null) return;
            
            foreach (Skeleton esqueleto in esqueletos)
            {
                if(esqueleto.TrackingState==SkeletonTrackingState.Tracked)
                {
                    //Articulaciones:
                    Joint Head = esqueleto.Joints[JointType.Head];
                    Joint ShoulderCenter = esqueleto.Joints[JointType.ShoulderCenter];
                    Joint ShoulderRight = esqueleto.Joints[JointType.ShoulderRight];
                    Joint ShoulderLeft = esqueleto.Joints[JointType.ShoulderLeft];
                    Joint ElbowRight= esqueleto.Joints[JointType.ElbowRight];
                    Joint ElbowLeft = esqueleto.Joints[JointType.ElbowLeft];
                    Joint WristRight = esqueleto.Joints[JointType.WristRight];
                    Joint WristLeft = esqueleto.Joints[JointType.WristLeft];
                    Joint HandRight = esqueleto.Joints[JointType.HandRight];
                    Joint HandLeft = esqueleto.Joints[JointType.HandLeft];
                    Joint Spine = esqueleto.Joints[JointType.Spine];
                    Joint HipCenter = esqueleto.Joints[JointType.HipCenter];
                    Joint HipRight = esqueleto.Joints[JointType.HipRight];
                    Joint HipLeft = esqueleto.Joints[JointType.HipLeft];
                    Joint KneeRight = esqueleto.Joints[JointType.KneeRight];
                    Joint KneeLeft = esqueleto.Joints[JointType.KneeLeft];
                    Joint AnkleRight = esqueleto.Joints[JointType.AnkleRight];
                    Joint FootRight = esqueleto.Joints[JointType.FootRight];
                    Joint AnkleLeft = esqueleto.Joints[JointType.AnkleLeft];
                    Joint FootLeft = esqueleto.Joints[JointType.FootLeft];
                    //LINEAS PARA  UNIR LAS ARTICULACIONES
                    Line HuesoCuello = new Line();
                    HuesoCuello.Stroke = new SolidColorBrush(Colors.LightSkyBlue);
                    HuesoCuello.StrokeThickness = 5;

                    Line HuesoCodoDerecho = new Line();
                    HuesoCodoDerecho.Stroke = new SolidColorBrush(Colors.LightSkyBlue);
                    HuesoCodoDerecho.StrokeThickness = 5;

                    Line HuesoCodoIzq = new Line();
                    HuesoCodoIzq.Stroke = new SolidColorBrush(Colors.LightSkyBlue);
                    HuesoCodoIzq.StrokeThickness = 5;

                    Line HuesoHombroDerecho = new Line();
                    HuesoHombroDerecho.Stroke = new SolidColorBrush(Colors.LightSkyBlue);
                    HuesoHombroDerecho.StrokeThickness = 5;

                    Line HuesoHombroIzquierdo = new Line();
                    HuesoHombroIzquierdo.Stroke = new SolidColorBrush(Colors.LightSkyBlue);
                    HuesoHombroIzquierdo.StrokeThickness = 5;

                    Line HuesoMunecaDer = new Line();
                    HuesoMunecaDer.Stroke = new SolidColorBrush(Colors.LightSkyBlue);
                    HuesoMunecaDer.StrokeThickness = 5;

                    Line HuesoMunecaIzq = new Line();
                    HuesoMunecaIzq.Stroke = new SolidColorBrush(Colors.LightSkyBlue);
                    HuesoMunecaIzq.StrokeThickness = 5;

                    Line HuesoManoDer = new Line();
                    HuesoManoDer.Stroke= new SolidColorBrush(Colors.LightSkyBlue);
                    HuesoManoDer.StrokeThickness = 5;

                    Line HuesoManoIzq=new Line();
                    HuesoManoIzq.Stroke = new SolidColorBrush(Colors.LightSkyBlue);
                    HuesoManoIzq.StrokeThickness = 5;

                    Line HuesoAbdomen = new Line();
                    HuesoAbdomen.Stroke = new SolidColorBrush(Colors.LightSkyBlue);
                    HuesoAbdomen.StrokeThickness = 5;

                    Line HuesoCadera = new Line();
                    HuesoCadera.Stroke = new SolidColorBrush(Colors.LightSkyBlue);
                    HuesoCadera.StrokeThickness = 5;

                    Line HuesoCaderaDer = new Line();
                    HuesoCaderaDer.Stroke = new SolidColorBrush(Colors.LightSkyBlue);
                    HuesoCaderaDer.StrokeThickness = 5;

                    Line HuesoCaderaIzq = new Line();
                    HuesoCaderaIzq.Stroke = new SolidColorBrush(Colors.LightSkyBlue);
                    HuesoCaderaIzq.StrokeThickness = 5;

                    Line HuesoRodiIzq = new Line();
                    HuesoRodiIzq.Stroke = new SolidColorBrush(Colors.LightSkyBlue);
                    HuesoRodiIzq.StrokeThickness = 5;

                    Line HuesoRodillaDer = new Line();
                    HuesoRodillaDer.Stroke = new SolidColorBrush(Colors.LightSkyBlue);
                    HuesoRodillaDer.StrokeThickness = 5;

                    Line HuesoTibiaDer = new Line();
                    HuesoTibiaDer.Stroke = new SolidColorBrush(Colors.LightSkyBlue);
                    HuesoTibiaDer.StrokeThickness = 5;

                    Line HuesoPieDer = new Line();
                    HuesoPieDer.Stroke = new SolidColorBrush(Colors.LightSkyBlue);
                    HuesoPieDer.StrokeThickness = 5;

                    Line HuesoPieIzq = new Line();
                    HuesoPieIzq.Stroke = new SolidColorBrush(Colors.LightSkyBlue);
                    HuesoPieIzq.StrokeThickness = 5;

                    Line HuesoRodillaIzq= new Line();
                    HuesoCaderaIzq.Stroke = new SolidColorBrush(Colors.LightSkyBlue);
                    HuesoCaderaIzq.StrokeThickness = 5;

                    Line HuesoTibiaIzq = new Line();
                    HuesoTibiaIzq.Stroke = new SolidColorBrush(Colors.LightSkyBlue);
                    HuesoTibiaIzq.StrokeThickness = 5;

                    //PUNTOS DE LAS ARTICULACIONES
                    ColorImagePoint PuntoCabeza = MiKinect.CoordinateMapper.MapSkeletonPointToColorPoint(Head.Position, ColorImageFormat.RgbResolution640x480Fps30);
                    HuesoCuello.X1 = PuntoCabeza.X;
                    HuesoCuello.Y1 = PuntoCabeza.Y;
                    ColorImagePoint PuntoCuello = MiKinect.CoordinateMapper.MapSkeletonPointToColorPoint(ShoulderCenter.Position, ColorImageFormat.RgbResolution640x480Fps30);
                    HuesoCuello.X2 = PuntoCuello.X;
                    HuesoCuello.Y2 = PuntoCuello.Y;
                    HuesoHombroDerecho.X1 = PuntoCuello.X;
                    HuesoHombroDerecho.Y1 = PuntoCuello.Y;
                    HuesoHombroIzquierdo.X1 = PuntoCuello.X;
                    HuesoHombroIzquierdo.Y1 = PuntoCuello.Y;
                    HuesoAbdomen.X1 = PuntoCuello.X;
                    HuesoAbdomen.Y1 = PuntoCuello.Y;
                    ColorImagePoint PuntoHombroDer = MiKinect.CoordinateMapper.MapSkeletonPointToColorPoint(ShoulderRight.Position, ColorImageFormat.RgbResolution640x480Fps30);
                    HuesoHombroDerecho.X2 = PuntoHombroDer.X;
                    HuesoHombroDerecho.Y2 = PuntoHombroDer.Y;
                    HuesoCodoDerecho.X1 = PuntoHombroDer.X;
                    HuesoCodoDerecho.Y1 = PuntoHombroDer.Y;
                    ColorImagePoint PuntoAbdomen = MiKinect.CoordinateMapper.MapSkeletonPointToColorPoint(Spine.Position, ColorImageFormat.RgbResolution640x480Fps30);
                    HuesoAbdomen.X2 = PuntoAbdomen.X;
                    HuesoAbdomen.Y2 = PuntoAbdomen.Y;
                    HuesoCadera.X1=PuntoAbdomen.X;
                    HuesoCadera.Y1=PuntoAbdomen.Y;
                    ColorImagePoint PuntoCadera = MiKinect.CoordinateMapper.MapSkeletonPointToColorPoint(HipCenter.Position, ColorImageFormat.RgbResolution640x480Fps30);
                    HuesoCadera.X2 = PuntoCadera.X;
                    HuesoCadera.Y2 = PuntoCadera.Y;
                    HuesoCaderaDer.X1 = PuntoCadera.X;
                    HuesoCaderaDer.Y1 = PuntoCadera.Y;
                    HuesoCaderaIzq.X1 = PuntoCadera.X;
                    HuesoCaderaIzq.Y1 = PuntoCadera.Y;
                    ColorImagePoint PuntoCaderaIzq = MiKinect.CoordinateMapper.MapSkeletonPointToColorPoint(HipLeft.Position, ColorImageFormat.RgbResolution640x480Fps30);
                    HuesoCaderaIzq.X2 = PuntoCaderaIzq.X;
                    HuesoCaderaIzq.Y2 = PuntoCaderaIzq.Y;
                    HuesoRodiIzq.X1 = PuntoCaderaIzq.X;
                    HuesoRodiIzq.Y1 = PuntoCaderaIzq.Y;
                    ColorImagePoint PuntoRodillaIzq = MiKinect.CoordinateMapper.MapSkeletonPointToColorPoint(KneeLeft.Position, ColorImageFormat.RgbResolution640x480Fps30);
                    HuesoRodiIzq.X2 = PuntoRodillaIzq.X;
                    HuesoRodiIzq.Y2 = PuntoRodillaIzq.Y;
                    HuesoTibiaIzq.X1 = PuntoRodillaIzq.X;
                    HuesoTibiaIzq.Y1 = PuntoRodillaIzq.Y;
                    ColorImagePoint PuntoTibiaIzq = MiKinect.CoordinateMapper.MapSkeletonPointToColorPoint(AnkleLeft.Position, ColorImageFormat.RgbResolution640x480Fps30);
                    HuesoTibiaIzq.X2 = PuntoTibiaIzq.X;
                    HuesoTibiaIzq.Y2 = PuntoTibiaIzq.Y;
                    HuesoPieIzq.X1 = PuntoTibiaIzq.X;
                    HuesoPieIzq.Y1 = PuntoTibiaIzq.Y;
                    ColorImagePoint PuntoPieIzq = MiKinect.CoordinateMapper.MapSkeletonPointToColorPoint(FootLeft.Position, ColorImageFormat.RgbResolution640x480Fps30);
                    HuesoPieIzq.X2 = PuntoPieIzq.X;
                    HuesoPieIzq.Y2 = PuntoPieIzq.Y;

                    ColorImagePoint PuntoCaderaDer = MiKinect.CoordinateMapper.MapSkeletonPointToColorPoint(HipRight.Position, ColorImageFormat.RgbResolution640x480Fps30);
                    HuesoCaderaDer.X2 = PuntoCaderaDer.X;
                    HuesoCaderaDer.Y2= PuntoCaderaDer.Y;
                    HuesoRodillaDer.X1 = PuntoCaderaDer.X;
                    HuesoRodillaDer.Y1 = PuntoCaderaDer.Y;
                    ColorImagePoint PuntoRodillaDer = MiKinect.CoordinateMapper.MapSkeletonPointToColorPoint(KneeRight.Position, ColorImageFormat.RgbResolution640x480Fps30);
                    HuesoRodillaDer.X2 = PuntoRodillaDer.X;
                    HuesoRodillaDer.Y2 = PuntoRodillaDer.Y;
                    HuesoTibiaDer.X1 = PuntoRodillaDer.X;
                    HuesoTibiaDer.Y1 = PuntoRodillaDer.Y;
                    ColorImagePoint PuntoTibiaDer = MiKinect.CoordinateMapper.MapSkeletonPointToColorPoint(AnkleRight.Position, ColorImageFormat.RgbResolution640x480Fps30);
                    HuesoTibiaDer.X2 = PuntoTibiaDer.X;
                    HuesoTibiaDer.Y2 = PuntoTibiaDer.Y;
                    HuesoPieDer.X1 = PuntoTibiaDer.X;
                    HuesoPieDer.Y1 = PuntoTibiaDer.Y;
                    ColorImagePoint PuntoPieDer = MiKinect.CoordinateMapper.MapSkeletonPointToColorPoint(FootRight.Position, ColorImageFormat.RgbResolution640x480Fps30);
                    HuesoPieDer.X2 = PuntoPieDer.X;
                    HuesoPieDer.Y2 = PuntoPieDer.Y;
                    ColorImagePoint PuntoHombroIzq = MiKinect.CoordinateMapper.MapSkeletonPointToColorPoint(ShoulderLeft.Position, ColorImageFormat.RgbResolution640x480Fps30);
                    HuesoHombroIzquierdo.X2 = PuntoHombroIzq.X;
                    HuesoHombroIzquierdo.Y2 = PuntoHombroIzq.Y;
                    HuesoCodoIzq.X1 = PuntoHombroIzq.X;
                    HuesoCodoIzq.Y1 = PuntoHombroIzq.Y;
                    ColorImagePoint PuntoCodoIzq = MiKinect.CoordinateMapper.MapSkeletonPointToColorPoint(ElbowLeft.Position, ColorImageFormat.RgbResolution640x480Fps30);
                    HuesoCodoIzq.X2 = PuntoCodoIzq.X;
                    HuesoCodoIzq.Y2 = PuntoCodoIzq.Y;
                    HuesoMunecaIzq.X1 = PuntoCodoIzq.X;
                    HuesoMunecaIzq.Y1 = PuntoCodoIzq.Y;
                    ColorImagePoint PuntoMunecaIzq = MiKinect.CoordinateMapper.MapSkeletonPointToColorPoint(WristLeft.Position, ColorImageFormat.RgbResolution640x480Fps30);
                    HuesoMunecaIzq.X2 = PuntoMunecaIzq.X;
                    HuesoMunecaIzq.Y2 = PuntoMunecaIzq.Y;
                    HuesoManoIzq.X1 = PuntoMunecaIzq.X;
                    HuesoManoIzq.Y1 = PuntoMunecaIzq.Y;
                    ColorImagePoint PuntoManoIzq=  MiKinect.CoordinateMapper.MapSkeletonPointToColorPoint(HandLeft.Position, ColorImageFormat.RgbResolution640x480Fps30); 
                    HuesoManoIzq.X2= PuntoManoIzq.X;
                    HuesoManoIzq.Y2=PuntoManoIzq.Y;
                    ColorImagePoint PuntoCodoDer = MiKinect.CoordinateMapper.MapSkeletonPointToColorPoint(ElbowRight.Position, ColorImageFormat.RgbResolution640x480Fps30);
                    HuesoCodoDerecho.X2 = PuntoCodoDer.X;
                    HuesoCodoDerecho.Y2 = PuntoCodoDer.Y;
                    HuesoMunecaDer.X1 = PuntoCodoDer.X;
                    HuesoMunecaDer.Y1 = PuntoCodoDer.Y;
                    ColorImagePoint PuntoMunecaDer = MiKinect.CoordinateMapper.MapSkeletonPointToColorPoint(WristRight.Position, ColorImageFormat.RgbResolution640x480Fps30);
                    HuesoMunecaDer.X2 = PuntoMunecaDer.X;
                    HuesoMunecaDer.Y2 = PuntoMunecaDer.Y;
                    HuesoManoDer.X1 = PuntoMunecaDer.X;
                    HuesoManoDer.Y1 = PuntoMunecaDer.Y;
                    ColorImagePoint PuntoManoDer = MiKinect.CoordinateMapper.MapSkeletonPointToColorPoint(WristRight.Position, ColorImageFormat.RgbResolution640x480Fps30);
                    HuesoManoDer.X2 = PuntoManoDer.X;
                    HuesoManoDer.Y2 = PuntoManoDer.Y;

                    //MOSTRANDO RESULTADOS:
                    //Realizar el calculo de la mano derecha con respecto al hombro
                    Joint handRigthtt = esqueleto.Joints[JointType.HandRight];
                    Joint shoulderrightt = esqueleto.Joints[JointType.ShoulderRight];
                    Joint elbowghtt = esqueleto.Joints[JointType.ElbowRight];
                    ////////////////////////////////////////////////////////////////////////////////
                    Joint handLeftt = esqueleto.Joints[JointType.HandLeft];
                    Joint shoulderlefft = esqueleto.Joints[JointType.ShoulderLeft];
                    Joint elbowLeft = esqueleto.Joints[JointType.ElbowLeft];

                    if ((handRigthtt.TrackingState == JointTrackingState.Tracked) && (shoulderrightt.TrackingState == JointTrackingState.Tracked))
                    {
                        this.Angulos(handRigthtt, elbowghtt, shoulderrightt);

                        this.ObtenerDistancia(handRigthtt, shoulderrightt);
                        Console.WriteLine("La distancia es:" + ObtenerDistancia(handRigthtt, shoulderrightt));
                    }

                    if ((handLeftt.TrackingState == JointTrackingState.Tracked) && (shoulderlefft.TrackingState == JointTrackingState.Tracked))
                    {
                        this.Angulos(handLeftt, elbowLeft, shoulderlefft);

                        this.ObtenerDistancia(handLeftt,shoulderlefft);
                        
                    }

                    string mensaje;
                    string anguloss;
                    string setangleLeft,setdistanceLeft;
                    setangleLeft = "" + Angulos(handLeftt, elbowLeft, shoulderlefft);
                    anguloss = "" + Angulos(handRigthtt, elbowghtt, shoulderrightt);
                    mensaje = "" + ObtenerDistancia(handRigthtt, shoulderrightt);
                    setdistanceLeft = "" + ObtenerDistancia(handLeftt, shoulderlefft);
                    AngleRight.Text = anguloss;
                    AngleLeft.Text = setangleLeft;
                    DistanceRight.Text = mensaje;
                    DistanceLeft.Text = setdistanceLeft;

                    /////////////////////////////////////////////

                   canvasesqueleto.Children.Add(HuesoCuello);
                   canvasesqueleto.Children.Add(HuesoHombroDerecho);
                   canvasesqueleto.Children.Add(HuesoCodoDerecho);
                   canvasesqueleto.Children.Add(HuesoMunecaDer);
                   canvasesqueleto.Children.Add(HuesoManoDer);
                   canvasesqueleto.Children.Add(HuesoHombroIzquierdo);
                   canvasesqueleto.Children.Add(HuesoCodoIzq);
                   canvasesqueleto.Children.Add(HuesoMunecaIzq);
                   canvasesqueleto.Children.Add(HuesoManoIzq);
                   canvasesqueleto.Children.Add(HuesoAbdomen);
                   canvasesqueleto.Children.Add(HuesoCadera);
                   canvasesqueleto.Children.Add(HuesoCaderaDer);
                   canvasesqueleto.Children.Add(HuesoRodillaDer);
                   canvasesqueleto.Children.Add(HuesoTibiaDer);
                   canvasesqueleto.Children.Add(HuesoPieDer);
                   canvasesqueleto.Children.Add(HuesoCaderaIzq);
                    canvasesqueleto.Children.Add(HuesoRodiIzq);
                    canvasesqueleto.Children.Add(HuesoTibiaIzq);
                    canvasesqueleto.Children.Add(HuesoPieIzq);
                }
            }
        }
        }
    }

