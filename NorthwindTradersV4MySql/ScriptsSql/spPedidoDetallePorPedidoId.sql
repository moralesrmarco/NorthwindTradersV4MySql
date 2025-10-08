CREATE PROCEDURE `spPedidoDetallePorPedidoId` (IN pPedidoId int)
BEGIN
	SELECT
    `od`.`OrderID`,
    `od`.`ProductID`,
    `p`.`ProductName`,
    `od`.`UnitPrice`,
    `od`.`Quantity`,
    `od`.`Discount`,
    `od`.`RowVersion`
  FROM
    `Order Details` AS `od`
    INNER JOIN `Products` AS `p` ON `od`.`ProductID` = `p`.`ProductID`
  WHERE
    `od`.`OrderID` = `pPedidoId`
  ORDER BY
    `od`.`ProductID`;
END
