using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using iGMS.Models;

namespace iGMS
{
    public class Dele
    {
  
        public static void DeleteGood(string idGood)
        {
            iPOSEntities db = new iPOSEntities();
            var countDeWa = db.DetailWareHouses.Where(x => x.IdGoods == idGood).Count();
            for (int i = 0; i < countDeWa; i++)
            {
                var idDeWa = db.DetailWareHouses.OrderBy(x => x.IdGoods == idGood).ToList().LastOrDefault().Id;
                DeleteDetailWareHouses(idDeWa);
            }
            var countDeGo = db.DetailGoodOrders.Where(x => x.IdGoods == idGood).Count();
            for (int i = 0; i < countDeGo; i++)
            {
                var idDeGo = db.DetailGoodOrders.OrderBy(x => x.IdGoods == idGood).ToList().LastOrDefault().Id;
                DeleteDetailGoodOrders(idDeGo);
            }
            var countEpc = db.EPCs.Where(x => x.IdGoods == idGood).Count();
            for (int i = 0; i < countEpc; i++)
            {
                var idEpc = db.EPCs.OrderBy(x => x.IdGoods == idGood).ToList().LastOrDefault().Id;
                DeleteEPCs(idEpc);
            }
            var countDeSa = db.DetailSaleOrders.Where(x => x.IdGoods == idGood).Count();
            for (int i = 0; i < countDeSa; i++)
            {
                var idDeSa = db.DetailSaleOrders.OrderBy(x => x.IdGoods == idGood).ToList().LastOrDefault().Id;
                DeleteDetailSaleOrders(idDeSa);
            }
            var countDeTra = db.DetailTransferOrders.Where(x => x.IdGoods == idGood).Count();
            for (int i = 0; i < countDeTra; i++)
            {
                var idDeTra = db.DetailTransferOrders.OrderBy(x => x.IdGoods == idGood).ToList().LastOrDefault().Id;
                DeleteDetailTransferOrders(idDeTra);
            }
            var countDeBi = db.DetailBills.Where(x => x.IdGoods == idGood).Count();
            for (int i = 0; i < countDeBi; i++)
            {
                var idDeBi = db.DetailBills.OrderBy(x => x.IdGoods == idGood).ToList().LastOrDefault().Id;
                DeleteDetailBills(idDeBi);
            }
            var countDeSuGo = db.DetailSupplierGoods.Where(x => x.IdGoods == idGood).Count();
            for (int i = 0; i < countDeSuGo; i++)
            {
                var idDeSuGo = db.DetailSupplierGoods.OrderBy(x => x.IdGoods == idGood).ToList().LastOrDefault().Id;
                DeleteDetailBills(idDeSuGo);
            }
            var countDeRe = db.DetailReceipts.Where(x => x.idGood == idGood).Count();
            for (int i = 0; i < countDeRe; i++)
            {
                var idDeRe = db.DetailReceipts.OrderBy(x => x.idGood == idGood).ToList().LastOrDefault().Id;
                DeleteDetailReceipts(idDeRe);
            }
            var countOrUn = db.OrderUnits.Where(x => x.IdGoods == idGood).Count();
            for (int i = 0; i < countOrUn; i++)
            {
                var idOrUn = db.OrderUnits.OrderBy(x => x.IdGoods == idGood).ToList().LastOrDefault().Id;
                DeleteOrderUnits(idOrUn);
            }
            var good = db.Goods.Find(idGood);
            db.Goods.Remove(good);
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update the values of the entity that failed to save from the store
                    ex.Entries.Single().Reload();
                }

            } while (saveFailed);
        }
        public static void DeleteDetailSaleOrders(int idDeSa)
        {
            iPOSEntities db = new iPOSEntities();
            db.Configuration.ProxyCreationEnabled = false;
            var deSa = db.DetailSaleOrders.Find(idDeSa);
            db.DetailSaleOrders.Remove(deSa);
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update the values of the entity that failed to save from the store
                    ex.Entries.Single().Reload();
                }

            } while (saveFailed);
        }
        public static void DeleteDetailTransferOrders(int idDeTra)
        {
            iPOSEntities db = new iPOSEntities();
            var deTra = db.DetailTransferOrders.Find(idDeTra);
            db.DetailTransferOrders.Remove(deTra);
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update the values of the entity that failed to save from the store
                    ex.Entries.Single().Reload();
                }

            } while (saveFailed);
        }
        public static void DeleteDetailGoodOrders(int idDeGo)
        {
            iPOSEntities db = new iPOSEntities();
            var deGo = db.DetailGoodOrders.Find(idDeGo);
            db.DetailGoodOrders.Remove(deGo);
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update the values of the entity that failed to save from the store
                    ex.Entries.Single().Reload();
                }

            } while (saveFailed);
        }
        public static void DeleteDetailBills(int idDeBi)
        {
            iPOSEntities db = new iPOSEntities();
            var deBi = db.DetailBills.Find(idDeBi);
            db.DetailBills.Remove(deBi);
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update the values of the entity that failed to save from the store
                    ex.Entries.Single().Reload();
                }

            } while (saveFailed);
        }
        public static void DeleteDetailSupplierGoods(int idDeSuGo)
        {
            iPOSEntities db = new iPOSEntities();
            var DeSuGo = db.DetailSupplierGoods.Find(idDeSuGo);
            db.DetailSupplierGoods.Remove(DeSuGo);
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update the values of the entity that failed to save from the store
                    ex.Entries.Single().Reload();
                }

            } while (saveFailed);
        } 
        public static void DeleteDetailReceipts(int idDeRe)
        {
            iPOSEntities db = new iPOSEntities();
            var DeRe = db.DetailReceipts.Find(idDeRe);
            db.DetailReceipts.Remove(DeRe);
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update the values of the entity that failed to save from the store
                    ex.Entries.Single().Reload();
                }

            } while (saveFailed);
        } 
        public static void DeleteEPCs(int idEpc)
        {
            iPOSEntities db = new iPOSEntities();
            var epc = db.EPCs.Find(idEpc);
            db.EPCs.Remove(epc);
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update the values of the entity that failed to save from the store
                    ex.Entries.Single().Reload();
                }

            } while (saveFailed);
        } 
        public static void DeleteDetailWareHouses(int idDeWa)
        {
            iPOSEntities db = new iPOSEntities();
            var deWa = db.DetailWareHouses.Find(idDeWa);
            db.DetailWareHouses.Remove(deWa);
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update the values of the entity that failed to save from the store
                    ex.Entries.Single().Reload();
                }

            } while (saveFailed);
        } 
        public static void DeletePromotions(int idPr)
        {
            iPOSEntities db = new iPOSEntities();
            var Pr = db.Promotions.Find(idPr);
            db.Promotions.Remove(Pr);
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update the values of the entity that failed to save from the store
                    ex.Entries.Single().Reload();
                }

            } while (saveFailed);
        }
        public static void DeleteCateGoods(string idCa)
        {
            iPOSEntities db = new iPOSEntities();
            var cate = db.CateGoods.Find(idCa);
            db.CateGoods.Remove(cate);
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update the values of the entity that failed to save from the store
                    ex.Entries.Single().Reload();
                }

            } while (saveFailed);
        }
        public static void DeleteDeliveries(string idDeLi)
        {
            iPOSEntities db = new iPOSEntities();
            var deli = db.Deliveries.Find(idDeLi);
            db.Deliveries.Remove(deli);
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update the values of the entity that failed to save from the store
                    ex.Entries.Single().Reload();
                }

            } while (saveFailed);
        } 
        public static void DeleteReceipts(string idRece)
        {
            iPOSEntities db = new iPOSEntities();
            var countDeRe = db.DetailReceipts.Where(x => x.IdReceipt == idRece).Count();
            for (int i = 0; i < countDeRe; i++)
            {
                var idDeRe = db.DetailReceipts.OrderBy(x => x.IdReceipt == idRece).ToList().LastOrDefault().Id;
                DeleteDetailReceipts(idDeRe);
            }
            var receipt = db.Receipts.Find(idRece);
            db.Receipts.Remove(receipt);
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update the values of the entity that failed to save from the store
                    ex.Entries.Single().Reload();
                }

            } while (saveFailed);
        }
        public static void DeleteSalesOrders(int idSale)
        {
            iPOSEntities db = new iPOSEntities();
            var countDeSa = db.DetailSaleOrders.Where(x => x.IdSaleOrder == idSale).Count();
            for (int i = 0; i < countDeSa; i++)
            {
                var idDeSa = db.DetailSaleOrders.OrderBy(x => x.IdSaleOrder == idSale).ToList().LastOrDefault().Id;
                DeleteDetailSaleOrders(idDeSa);
            }
            var countDeLi = db.Deliveries.Where(x => x.IdSalesOrder == idSale).Count();
            for (int i = 0; i < countDeLi; i++)
            {
                var idDeLi = db.Deliveries.OrderBy(x => x.IdSalesOrder == idSale).ToList().LastOrDefault().Id;
                DeleteDeliveries(idDeLi);
            }
            var sale = db.SalesOrders.Find(idSale);
            db.SalesOrders.Remove(sale);
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update the values of the entity that failed to save from the store
                    ex.Entries.Single().Reload();
                }

            } while (saveFailed);
        }
        public static void DeletePurchaseOrders(int idPurchase)
        {
            iPOSEntities db = new iPOSEntities();
            var countDeTra = db.DetailTransferOrders.Where(x => x.IdPuchaseOrder == idPurchase).Count();
            for (int i = 0; i < countDeTra; i++)
            {
                var idDeTra = db.DetailTransferOrders.OrderBy(x => x.IdPuchaseOrder == idPurchase).ToList().LastOrDefault().Id;
                DeleteDetailTransferOrders(idDeTra);
            }
            var countDeGo = db.DetailGoodOrders.Where(x => x.IdPurchaseOrder == idPurchase).Count();
            for (int i = 0; i < countDeGo; i++)
            {
                var idDeGo = db.DetailGoodOrders.OrderBy(x => x.IdPurchaseOrder == idPurchase).ToList().LastOrDefault().Id;
                DeleteDetailGoodOrders(idDeGo);
            }
            var countRece = db.Receipts.Where(x => x.IdPurchaseOrder == idPurchase).Count();
            for (int i = 0; i < countRece; i++)
            {
                var idRece = db.Receipts.OrderBy(x => x.IdPurchaseOrder == idPurchase).ToList().LastOrDefault().Id;
                DeleteReceipts(idRece);
            }
            var purchase = db.PurchaseOrders.Find(idPurchase);
            db.PurchaseOrders.Remove(purchase);
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update the values of the entity that failed to save from the store
                    ex.Entries.Single().Reload();
                }

            } while (saveFailed);
        } 
        public static void DeleteWareHouses(string idWaHo)
        {
            iPOSEntities db = new iPOSEntities();
            var countGood = db.Goods.Where(x => x.IdWareHouse == idWaHo).Count();
            for (int i = 0; i < countGood; i++)
            {
                var idGood = db.Goods.OrderBy(x => x.IdWareHouse == idWaHo).ToList().LastOrDefault().Id;
                Dele.DeleteGood(idGood);
            }
            var countSale = db.SalesOrders.Where(x => x.IdWareHouse == idWaHo).Count();
            for (int i = 0; i < countSale; i++)
            {
                var idSale = db.SalesOrders.OrderBy(x => x.IdWareHouse == idWaHo).ToList().LastOrDefault().Id;
                Dele.DeleteSalesOrders(idSale);
            }
            var countPurchase = db.PurchaseOrders.Where(x => x.IdWareHouse == idWaHo).Count();
            for (int i = 0; i < countPurchase; i++)
            {
                var idPurchase = db.PurchaseOrders.OrderBy(x => x.IdWareHouse == idWaHo).ToList().LastOrDefault().Id;
                Dele.DeletePurchaseOrders(idPurchase);
            }
            var countDeWa = db.DetailWareHouses.Where(x => x.IdWareHouse == idWaHo).Count();
            for (int i = 0; i < countDeWa; i++)
            {
                var idDeWa = db.DetailWareHouses.OrderBy(x => x.IdWareHouse == idWaHo).ToList().LastOrDefault().Id;
                Dele.DeleteDetailWareHouses(idDeWa);
            }
            var countDeli = db.Deliveries.Where(x => x.IdWareHouse == idWaHo).Count();
            for (int i = 0; i < countDeli; i++)
            {
                var idDeli = db.Deliveries.OrderBy(x => x.IdWareHouse == idWaHo).ToList().LastOrDefault().Id;
                Dele.DeleteDeliveries(idDeli);
            }
            var d = db.WareHouses.Find(idWaHo);
            db.WareHouses.Remove(d);
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update the values of the entity that failed to save from the store
                    ex.Entries.Single().Reload();
                }

            } while (saveFailed);
        }
        public static void DeleteUnits(string idUnit)
        {
            iPOSEntities db = new iPOSEntities();
            var countGo = db.Goods.Where(x => x.IdUnit == idUnit).Count();
            for (int i = 0; i < countGo; i++)
            {
                var idGo = db.Goods.OrderBy(x => x.IdUnit == idUnit).ToList().LastOrDefault().Id;
                Dele.DeleteGood(idGo);
            }
            var countOrUn = db.OrderUnits.Where(x => x.IdUnit == idUnit).Count();
            for (int i = 0; i < countOrUn; i++)
            {
                var idOrUn = db.OrderUnits.OrderBy(x => x.IdUnit == idUnit).ToList().LastOrDefault().Id;
                DeleteOrderUnits(idOrUn);
            }
            var unit = db.Units.Find(idUnit);
            db.Units.Remove(unit);
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update the values of the entity that failed to save from the store
                    ex.Entries.Single().Reload();
                }

            } while (saveFailed);
        }
        public static void DeleteStyles(string idStyle)
        {
            iPOSEntities db = new iPOSEntities();
            var countGo = db.Goods.Where(x => x.IdStyle == idStyle).Count();
            for (int i = 0; i < countGo; i++)
            {
                var idGo = db.Goods.OrderBy(x => x.IdStyle == idStyle).ToList().LastOrDefault().Id;
                Dele.DeleteGood(idGo);
            }
            var style = db.Styles.Find(idStyle);
            db.Styles.Remove(style);
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update the values of the entity that failed to save from the store
                    ex.Entries.Single().Reload();
                }

            } while (saveFailed);
        }
        public static void DeleteColors(string idColor)
        {
            iPOSEntities db = new iPOSEntities();
            var countGo = db.Goods.Where(x => x.IdColor == idColor).Count();
            for (int i = 0; i < countGo; i++)
            {
                var idGo = db.Goods.OrderBy(x => x.IdColor == idColor).ToList().LastOrDefault().Id;
                Dele.DeleteGood(idGo);
            }
            var color = db.Colors.Find(idColor);
            db.Colors.Remove(color);
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update the values of the entity that failed to save from the store
                    ex.Entries.Single().Reload();
                }

            } while (saveFailed);
        }
        public static void DeleteSizes(string idSize)
        {
            iPOSEntities db = new iPOSEntities();
            var countGo = db.Goods.Where(x => x.IdSize == idSize).Count();
            for (int i = 0; i < countGo; i++)
            {
                var idGo = db.Goods.OrderBy(x => x.IdSize == idSize).ToList().LastOrDefault().Id;
                Dele.DeleteGood(idGo);
            }
            var size = db.Sizes.Find(idSize);
            db.Sizes.Remove(size);
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update the values of the entity that failed to save from the store
                    ex.Entries.Single().Reload();
                }

            } while (saveFailed);
        }
        public static void DeleteNatures(string idNature)
        {
            iPOSEntities db = new iPOSEntities();
            var countGo = db.Goods.Where(x => x.IdNature == idNature).Count();
            for (int i = 0; i < countGo; i++)
            {
                var idGo = db.Goods.OrderBy(x => x.IdNature == idNature).ToList().LastOrDefault().Id;
                Dele.DeleteGood(idGo);
            }
            var nature = db.Natures.Find(idNature);
            db.Natures.Remove(nature);
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update the values of the entity that failed to save from the store
                    ex.Entries.Single().Reload();
                }

            } while (saveFailed);
        }
        public static void DeleteMaterialAccounts(string idMaAc)
        {
            iPOSEntities db = new iPOSEntities();
            var countGo = db.Goods.Where(x => x.IdMaterialAccount == idMaAc).Count();
            for (int i = 0; i < countGo; i++)
            {
                var idGo = db.Goods.OrderBy(x => x.IdMaterialAccount == idMaAc).ToList().LastOrDefault().Id;
                Dele.DeleteGood(idGo);
            }
            var materialAccount = db.MaterialAccounts.Find(idMaAc);
            db.MaterialAccounts.Remove(materialAccount);
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update the values of the entity that failed to save from the store
                    ex.Entries.Single().Reload();
                }

            } while (saveFailed);
        }
        public static void DeleteOrderUnits(int idOrUn)
        {
            iPOSEntities db = new iPOSEntities();
            var orUn = db.OrderUnits.Find(idOrUn);
            db.OrderUnits.Remove(orUn);
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update the values of the entity that failed to save from the store
                    ex.Entries.Single().Reload();
                }

            } while (saveFailed);
        }
    }
}