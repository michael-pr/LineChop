package models.database

import play.api.db.slick.Config.driver.simple._
import models.Queue

class Queues extends Table[Queue]("QUEUE") {

  def id = column[String]("ID")
  def name = column[String]("NAME")
  def hostId = column[Long]("HOST_ID")

  def * = id ~ name ~ hostId <> (Queue.apply _, Queue.unapply _)

}