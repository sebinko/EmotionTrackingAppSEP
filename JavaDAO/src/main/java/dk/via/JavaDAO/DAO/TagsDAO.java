package dk.via.JavaDAO.DAO;

import dk.via.JavaDAO.Models.Tag;

public interface TagsDAO {
  Tag GetSingle(int id);
  Tag Delete(Tag tag);
}
