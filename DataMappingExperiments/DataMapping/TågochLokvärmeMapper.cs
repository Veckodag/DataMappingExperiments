using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMappingExperiments.BisObjekt;
using DataMappingExperiments.Helpers;

namespace DataMappingExperiments.DataMapping
{
    class TågochLokvärmeMapper : Mapper
    {
        public TågochLokvärmeMapper()
        {
            MapperType = MapperType.TågOchLokVärmeanläggning;
            ExtraCounter = 1;
        }

        public override MapperType MapperType { get; set; }

        public override BIS_GrundObjekt MapXmlValue(int index, string attributeValue, BIS_GrundObjekt bisObject)
        {
            var myTåg = (BIS_TågOchLokvärmeanläggning) bisObject;
            switch (index)
            {
                case 0:
                    myTåg.ObjektTypNummer = attributeValue;
                    break;
                case 1:
                    myTåg.ObjektNummer = attributeValue;
                    break;
                case 29:
                    myTåg.Typ = attributeValue;
                    break;
                case 31:
                    myTåg.TransformatorEffekt = attributeValue;
                    break;
                case 34:
                    myTåg.TransformatorBrytareTyp = attributeValue;
                    break;
                case 36:
                    myTåg.Senast_Ändrad = attributeValue;
                    break;
                case 37:
                    myTåg.Senast_Ändrad_Av = attributeValue;
                    break;
                case 38:
                    myTåg.Notering = attributeValue;
                    break;
            }

            return myTåg;
        }

        public override Container ObjectStructure(List<BIS_GrundObjekt> bisList)
        {
            Container container = new Container();
            var containerSofttypes = new List<SoftType>();

            var anläggningsprodukter = new List<AnläggningsproduktInstances>();
            var specifikationer = new List<AnläggningsspecifikationInstances>();
            var styckevaror = new List<StyckevaraInstances>();

            foreach (BIS_TågOchLokvärmeanläggning bisTåg in bisList)
            {
                var suffix = bisTåg.ObjektTypNummer + bisTåg.ObjektNummer + ExtraCounter;
                var tågvärmepostspecifikation = CreateTågVärmePostSpecifikation(bisTåg, suffix);
                var tågvärmepostprodukt = CreateTågvärmepostprodukt(bisTåg, suffix);
                var tågvärmepostindivid = CreateTågvärmepostindivid(bisTåg, suffix);
                var transformatorbrytareprodukt = CreateTransformatorbrytareprodukt(bisTåg, suffix);

                var tågvärmepostspecifikationEntry = new AnläggningsspecifikationEntrydefaultIn()
                {
                    Array = true,
                    inputSchemaRef = _InputSchemaRef,
                    id = "tågvärmepostspecifikationEntry" + suffix,
                    data = tågvärmepostspecifikation
                };
                specifikationer.Add(tågvärmepostspecifikationEntry);

                var tågvärmepostproduktEntry = new AnläggningsproduktEntrydefaultIn()
                {
                    Array = true,
                    inputSchemaRef = _InputSchemaRef,
                    id = "tågvärmepostproduktEntry" + suffix,
                    data = tågvärmepostprodukt
                };
                anläggningsprodukter.Add(tågvärmepostproduktEntry);

                var transformatorbrytareproduktEntry = new AnläggningsproduktEntrydefaultIn()
                {
                    Array = true,
                    inputSchemaRef = _InputSchemaRef,
                    id = "transformatorbrytareproduktEntry" + suffix,
                    data = transformatorbrytareprodukt
                };
                anläggningsprodukter.Add(transformatorbrytareproduktEntry);

                var tågvärmepostindividEntry = new StyckevaraEntrydefaultIn()
                {
                    Array = true,
                    inputSchemaRef = _InputSchemaRef,
                    id = "tågvärmepostindividEntry" + suffix,
                    data = tågvärmepostindivid
                };
                styckevaror.Add(tågvärmepostindividEntry);

                ExtraCounter++;
            }

            var anläggningsproduktsofttype = new SoftType_Anläggningsprodukt
            {
                Array = true,
                id = "Anläggningsprodukt",
                instances = anläggningsprodukter.ToArray()
            };

            var styckevarasofttype = new SoftType_Styckevara()
            {
                Array = true,
                id = "Styckevara",
                instances = styckevaror.ToArray()
            };
            var specifikationsofttype = new SoftType_Anläggningsspecifikation()
            {
                Array = true,
                id = "Specifikation",
                instances = specifikationer.ToArray()
            };
            containerSofttypes.Add(anläggningsproduktsofttype);
            containerSofttypes.Add(specifikationsofttype);
            containerSofttypes.Add(styckevarasofttype);

            container.softTypes = containerSofttypes.ToArray();

            return container;
        }

        private Tågvärmepostspecifikation CreateTågVärmePostSpecifikation(BIS_TågOchLokvärmeanläggning p, string suffix)
        {
            Tågvärmepostspecifikation o = new Tågvärmepostspecifikation
            {
                name = "Tågvärmepostspecifikation",
                versionId = _VersionId,
                notering = p.Notering,
                stringSet = new TågvärmepostspecifikationStringSet
                {
                    transformatorEffekt = new Tågvärmepostspecifikation_transformatorEffekt
                    {
                        JSonMapToPropertyName = _JsonMapToValue,
                        value = CreateTransformatorEffekt(p.TransformatorEffekt),
                        generalProperty = new transformatorEffekt
                        {
                            softType = _SoftTypeProperty,
                            instanceRef = "transformatorEffekt"
                        }
                    },
                    typ = new Tågvärmepostspecifikation_typ
                    {
                        value = p.Typ,
                        generalProperty = new typ
                        {
                            instanceRef = "typ",
                            softType = _SoftTypeProperty
                        },
                        JSonMapToPropertyName = _JsonMapToValue
                    }
                },
                företeelsetyp = new ClassificationReference_Anläggningsspecifikation_företeelsetyp
                {
                    @class = new FTAnläggningsspecifikationReference
                    {
                        softType = "FTAnläggningsspecifikationReference",
                        instanceRef = "FTAnläggningsspecifikationReference"
                    },
                    endSpecified = false,
                    startSpecified = false
                },
            };
            o.id = o.name + suffix;
            return o;
        }

        private Tågvärmepostprodukt CreateTågvärmepostprodukt(BIS_TågOchLokvärmeanläggning p, string suffix)
        {
            Tågvärmepostprodukt o = new Tågvärmepostprodukt
            {
                name = "Tågvärmepostprodukt",
                versionId = _VersionId,
                notering = p.Notering,
                företeelsetyp = new ClassificationReference_Anläggningsprodukt_företeelsetyp
                {
                    @class = new FTAnläggningsproduktReference
                    {
                        instanceRef = "FTAnläggningsproduktReference",
                        softType = "FTAnläggningsproduktReference"
                    },
                    endSpecified = false,
                    startSpecified = false
                }
            };
            o.id = o.name + suffix;
            return o;
        }

        private Tågvärmepostindivid CreateTågvärmepostindivid(BIS_TågOchLokvärmeanläggning p, string suffix)
        {
            Tågvärmepostindivid o = new Tågvärmepostindivid
            {
                name = "Tågvärmepostindivid",
                versionId = _VersionId,
                notering = p.Notering,
                företeelsetyp = new ClassificationReference_Styckevara_företeelsetyp
                {
                    @class = new FTStyckevaraReference
                    {
                        softType = "FTStyckevaraReference",
                        instanceRef = "FTStyckevaraReference"
                    },
                    endSpecified = false,
                    startSpecified = false
                }
            };
            o.id = o.name + suffix;
            return o;
        }

        private Transformatorbrytareprodukt CreateTransformatorbrytareprodukt(BIS_TågOchLokvärmeanläggning p, string suffix)
        {
            Transformatorbrytareprodukt o = new Transformatorbrytareprodukt
            {
                name = "Transformatorbrytareprodukt",
                versionId = _VersionId,
                notering = p.Notering,
                företeelsetyp = new ClassificationReference_Anläggningsprodukt_företeelsetyp
                {
                    @class = new FTAnläggningsproduktReference
                    {
                        instanceRef = "FTAnläggningsproduktReference",
                        softType = "FTAnläggningsproduktReference"
                    },
                    endSpecified = false,
                    startSpecified = false
                }
            };
            o.id = o.name + suffix;
            return o;
        }

        private string CreateTransformatorEffekt(string value)
        {
            switch (value.Trim().ToLower())
            {
                case "?":
                    value = "0";
                    break;
                case "1000kva":
                    value = "1000";
                    break;
                case "100kva":
                    value = "100";
                    break;
                case "1250kva":
                    value = "1250";
                    break;
                case "1750kva":
                    value = "1750";
                    break;
                case "200kva":
                    value = "200";
                    break;
                case "250kva":
                    value = "250";
                    break;
                case "30kva":
                    value = "30";
                    break;
                case "500kva":
                    value = "500";
                    break;
                case "50kva":
                    value = "50";
                    break;
                case "800kva":
                    value = "800";
                    break;
            }

            return value;
        }

        public override List<SoftType> CreateFTKeyReferenceSoftTypes()
        {
            throw new NotImplementedException();
        }
    }
}
