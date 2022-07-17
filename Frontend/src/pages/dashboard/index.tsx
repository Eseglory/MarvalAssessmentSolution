import { Button, Modal } from "components";
import { useAuth } from "contexts/AuthContext";
import { useGetAllUploadedRecords } from "hooks/queries/person";
import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { addPerson, deletePerson, updatePerson } from "services/person";
import { PersonI } from "types/client";
import AddUser from "./components/addUser/AddUser";
import DeleteUser from "./components/deleteUser/DeleteUser";
import UpdateUser from "./components/updateUser/updateUser";
import UploadCsv from "./components/uploadCsv";
import "./dashboard.scss";

const Dashboard = () => {
  const [isAddUserModal, setIsAddUserModal] = useState(false);
  const [isUpdateUserModal, setIsUpdateUserModal] = useState(false);
  const [isDeleteUserModal, setIsDeleteUserModal] = useState(false);
  const [isUploadCsvModal, setIsUploadCsvModal] = useState(false);
  const [selectedUser, setSelectedUser] = useState<PersonI>();
  const [isUpdateUserLoading, setIsUpdateUserLoading] = useState(false);
  const [isAddUserLoading, setIsAddUserLoading] = useState(false);
  const [isDeleteUserLoading, setIsDeleteUserLoading] = useState(false);
  const [isUploadCsvLoading, setIsUploadCsvLoading] = useState(false);
  const [shouldUpdateUser, setShouldUpdateUser] = useState<boolean>(true);
  const [showAlert, setShowAlert] = useState(false);

  const navigate = useNavigate();

  const { handleLogout, user } = useAuth();
  const { data: persons, isLoading: personsLoading } = useGetAllUploadedRecords(
    {
      enabled: shouldUpdateUser,
    }
  );

  useEffect(() => {
    let timer: NodeJS.Timeout;
    if (showAlert) {
      timer = setTimeout(() => {
        setShowAlert(false);
      }, 5000);
    }

    return () => clearInterval(timer);
  }, [showAlert]);

  const handleAddUserOpen = () => setIsAddUserModal(true);
  const handleAddUserClose = () => setIsAddUserModal(false);

  const handleUploadCsvOpen = () => setIsUploadCsvModal(true);
  const handleUploadCsvClose = () => setIsUploadCsvModal(false);

  const handleDeleteUserOpen = (id: number) => {
    const filteredUser = persons?.find((item) => item.identity === id);

    if (filteredUser) {
      setSelectedUser(filteredUser);
    }
    setIsDeleteUserModal(true);
  };
  const handleDeleteUserClose = () => setIsDeleteUserModal(false);

  const handleUpdateUserOpen = (id: number) => {
    const filteredUser = persons?.find((item) => item.identity === id);

    if (filteredUser) {
      setSelectedUser(filteredUser);
    }

    setIsUpdateUserModal(true);
  };
  const handleUpdateUserClose = () => setIsUpdateUserModal(false);

  const handleAddUser = async (values: PersonI) => {
    setIsAddUserLoading(true);
    setShouldUpdateUser(false);

    values.age = Number(values.age);
    values.active = "True";

    try {
      const res = await addPerson(values, user?.token || "");

      if (res) {
        setIsAddUserModal(false);
        setShouldUpdateUser(true);
      }
      setIsAddUserLoading(false);
    } catch (error) {
      setIsAddUserLoading(false);
      setShouldUpdateUser(true);
    }
  };

  const handleUpdateUser = async (values: PersonI) => {
    setIsUpdateUserLoading(true);
    setShouldUpdateUser(false);

    try {
      const res = await updatePerson(values, user?.token || "");

      if (res) {
        setIsUpdateUserModal(false);
        setShouldUpdateUser(true);
      }
      setIsUpdateUserLoading(false);
    } catch (error) {
      setIsUpdateUserLoading(false);
      setShouldUpdateUser(true);
    }
  };

  const handleDeleteUser = async (id: string) => {
    setIsDeleteUserLoading(true);
    setShouldUpdateUser(false);

    try {
      const res = await deletePerson(id, user?.token || "");

      if (res) {
        setIsDeleteUserModal(false);
        setShouldUpdateUser(true);
      }
      setIsDeleteUserLoading(false);
    } catch (error) {
      setIsDeleteUserLoading(false);
      setShouldUpdateUser(true);
    }
  };

  return (
    <div className="dashboard">
      <div className="dashboard__container">
        <div className="dashboard__top">
          <h1 className="dashboard__header">Dashboard</h1>
          <Button onClick={handleLogout} className="dashboard__btn">
            Logout
          </Button>
        </div>
        <div>
          <Button onClick={handleUploadCsvOpen} className="dashboard__btn">
            Upload Users
          </Button>
          <Button onClick={handleAddUserOpen} className="dashboard__btn">
            Add Single User
          </Button>
        </div>

        {showAlert && (
          <div className="dashboard__alertCsv">CSV file uploaded</div>
        )}
        <div className="table">
          <div className="table__head">
            <div className="table__rowItem">Name</div>
            <div className="table__rowItem">Gender</div>
            <div className="table__rowItem">Age</div>
            <div className="table__rowItem">Action</div>
          </div>
          {personsLoading && <div>Loading...</div>}
          {persons &&
            persons.map((item, index) => (
              <div className="table__row" key={index}>
                <div className="table__rowItem">
                  <span>
                    {item.firstName} {item.surname}
                  </span>
                </div>
                <div className="table__rowItem">{item.sex}</div>
                <div className="table__rowItem">{item.age}</div>
                <div className="table__rowItem">
                  <Button
                    onClick={() => handleUpdateUserOpen(item.identity)}
                    className="table__btn"
                  >
                    Update
                  </Button>
                  <Button
                    onClick={() => navigate(`/person/${item.identity}`)}
                    className="table__btn"
                  >
                    View
                  </Button>
                  <Button
                    onClick={() => handleDeleteUserOpen(item.identity)}
                    className="table__btn"
                  >
                    Delete
                  </Button>
                </div>
              </div>
            ))}
        </div>
      </div>
      <Modal isOpen={isAddUserModal} handleClose={handleAddUserClose}>
        <AddUser
          isAddUserLoading={isAddUserLoading}
          handleAddUser={handleAddUser}
        />
      </Modal>
      <Modal isOpen={isUpdateUserModal} handleClose={handleUpdateUserClose}>
        <UpdateUser
          isUpdateUserLoading={isUpdateUserLoading}
          selectedUser={selectedUser}
          handleUpdateUser={handleUpdateUser}
        />
      </Modal>
      <Modal isOpen={isDeleteUserModal} handleClose={handleDeleteUserClose}>
        <DeleteUser
          handleDeleteUser={handleDeleteUser}
          handleDeleteUserClose={handleDeleteUserClose}
          selectedUser={selectedUser}
          isDeleteUserLoading={isDeleteUserLoading}
        />
      </Modal>
      <Modal isOpen={isUploadCsvModal} handleClose={handleUploadCsvClose}>
        <UploadCsv
          handleUploadCsvClose={handleUploadCsvClose}
          isUploadCsvLoading={isUploadCsvLoading}
          setShowAlert={setShowAlert}
          setIsUploadCsvLoading={setIsUploadCsvLoading}
        />
      </Modal>
    </div>
  );
};

export default Dashboard;
