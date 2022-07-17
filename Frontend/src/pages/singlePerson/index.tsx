import { Button } from "components";
import { useAuth } from "contexts/AuthContext";
import { useGetSinglePerson } from "hooks/queries/person";
import { Path } from "navigations/routes";
import React from "react";
import { useParams } from "react-router-dom";
import "./singlePerson.scss";

const SinglePerson = () => {
  const params = useParams();

  const { handleLogout } = useAuth();
  const { data, isLoading } = useGetSinglePerson(params?.id || "");

  return (
    <div className="singlePerson">
      <div className="singlePerson__container">
        <div className="singlePerson__top">
          <h1 className="singlePerson__header">Person Data</h1>
          <Button onClick={handleLogout} className="singlePerson__btn">
            Logout
          </Button>
        </div>
        <div>
          <Button as="link" to={Path.Dashboard}>
            Go Back
          </Button>
        </div>
        {isLoading && <div className="singlePerson__loading">Loading...</div>}
        {data && (
          <div className="singlePerson__data">
            <div className="singlePerson__row">
              <span className="singlePerson__key">First Name:</span>
              <span className="singlePerson__value">{data?.firstName}</span>
            </div>
            <div className="singlePerson__row">
              <span className="singlePerson__key">Surname:</span>
              <span className="singlePerson__value">{data?.surname}</span>
            </div>
            <div className="singlePerson__row">
              <span className="singlePerson__key">Gender:</span>
              <span className="singlePerson__value">{data?.sex}</span>
            </div>
            <div className="singlePerson__row">
              <span className="singlePerson__key">Age:</span>
              <span className="singlePerson__value">{data?.age}</span>
            </div>
            <div className="singlePerson__row">
              <span className="singlePerson__key">Active:</span>
              <span className="singlePerson__value">{data?.active}</span>
            </div>
            <div className="singlePerson__row">
              <span className="singlePerson__key">Mobile:</span>
              <span className="singlePerson__value">{data?.mobile}</span>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default SinglePerson;
