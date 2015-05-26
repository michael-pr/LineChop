package models

import models.database.Queues
import play.api.Play.current
import play.api.db.slick.Config.driver.simple._
import play.api.db.slick.DB
import scala.slick.session.Session

case class Queue(
  id: String,
  title: String,
  hostId: Long)

object Queue {

  val table = new Queues

  def getAll = DB.withSession { implicit session: Session =>
    Query(table).list
  }

}