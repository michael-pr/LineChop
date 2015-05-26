package models

import models.database.Users
import play.api.Play.current
import play.api.db.slick.Config.driver.simple._
import play.api.db.slick.DB
import scala.slick.session.Session

case class User(
  userId: Long,
  email: String,
  passwordHash: String,
  firstName: String,
  lastName: String,
  phone: String,
  secondaryPhone: String)

object User {

  val table = new Users

  def getAll: List[User] = DB.withSession { implicit session: Session =>
    Query(table).list
  }

}