import React, { ReactNode, useEffect } from "react";
import cs from "classnames";
import "./modal.scss";
import { FaTimes } from "react-icons/fa";

interface Props {
  isOpen: boolean;
  handleClose: () => void;
  children?: ReactNode;
}

const Modal: React.FC<Props> = ({ handleClose, isOpen, children }) => {
  const modalClasses = cs("modal", {
    active: isOpen,
  });

  useEffect(() => {
    if (isOpen) {
      document.body.style.overflow = "hidden";
    } else {
      document.body.style.overflow = "visible";
    }
  }, [isOpen]);
  return (
    <div className={modalClasses}>
      <div className="modal__overlay" onClick={handleClose}></div>
      <div className="modal__box">
        <div className="modal__close" onClick={handleClose}>
          <FaTimes />
        </div>
        <div className="modal__content">{children}</div>
      </div>
    </div>
  );
};

export default Modal;
