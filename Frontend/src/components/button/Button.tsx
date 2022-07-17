import cs from "classnames";
import { ReactNode } from "react";
import { Link, LinkProps } from "react-router-dom";
import "./button.scss";

interface Props {
  children: ReactNode;
  fullWidth?: boolean;
  className?: string;
  loading?: boolean;
  disabled?: boolean;
  variant?:
    | "plain"
    | "primary"
    | "secondary"
    | "primary-outline"
    | "secondary-outline";
}

type MainBtnProps = Props & React.ButtonHTMLAttributes<HTMLButtonElement>;
type BtnLinkProps = Props & LinkProps;
type MergedProps = Omit<BtnLinkProps & MainBtnProps, "to">;
type ButtonProps = {
  as?: "button" | "link";
  to?: string;
} & MergedProps;

const Button: React.FC<ButtonProps> = ({
  as = "button",
  to = "/",
  ...props
}) => {
  return (
    <>
      {as === "link" ? <BtnLink to={to} {...props} /> : <MainBtn {...props} />}
    </>
  );
};

const MainBtn: React.FC<MainBtnProps> = ({
  children,
  variant = "primary",
  className,
  disabled,
  fullWidth,
  loading,
  ...props
}) => {
  const classes = cs("button", {
    [`${className}`]: className,
    "button--plain": variant === "plain",
    "button--primary": variant === "primary",
    "button--secondary": variant === "secondary",
    "button--primaryOutline": variant === "primary-outline",
    "button--secondaryOutline": variant === "secondary-outline",
    "button--fullWidth": fullWidth,
    "button--disabled": disabled,
    "button--loading": loading,
  });
  return (
    <button
      disabled={disabled || loading}
      data-testid="button"
      className={classes}
      {...props}
    >
      {children}
    </button>
  );
};

const BtnLink: React.FC<BtnLinkProps> = ({
  children,
  variant = "primary",
  className,
  disabled,
  fullWidth,
  ...props
}) => {
  const classes = cs("button", {
    [`${className}`]: className,
    "button--plain": variant === "plain",
    "button--primary": variant === "primary",
    "button--secondary": variant === "secondary",
    "button--primaryOutline": variant === "primary-outline",
    "button--secondaryOutline": variant === "secondary-outline",
    "button--fullWidth": fullWidth,
    "button--disabled": disabled,
  });
  return (
    <Link data-testid="button" className={classes} {...props}>
      {children}
    </Link>
  );
};

export default Button;
